using System;
using Enums;
using Interfaces;
using Inventories;
using Items;
using Items.Equipment.Weapons;
using Loot;
using Managers;
using Saves;
using StatusEffects;
using TMPro;
using UnityEngine;
using static Enums.ActionCode;

namespace Characters.Player
{
	public class Player : Character, IAttack
	{
		public TextMeshProUGUI levelText;
		public GameObject weaponObject;
		public Transform slashObject;
		public Weapon weapon;

		private bool _openChest;
		private LootChest _collidingChest;
		
		private Inventory _inventory;
		private InventoryManager _inventoryManager;

		private Collider2D _collider;
		
		/// <summary>
		/// Gives information about item added to inventory
		/// </summary>
		public event Action<Item> ItemCollected;
		public void InvokeItemCollected(Item item)
		{
			ItemCollected?.Invoke(item);

			//TODO if cant fit in inventory display little popup that asks if player wants to add collected item to vault
		}

		protected override void Awake()
		{
			base.Awake();

			for (var i = 0; i < stats.Count; i++)
			{
				var stat = stats[i];
				stat.StatsList = GameManager.Instance.InventoryManager.statsList;
				stat.StatsList.stats[i].UpdateText(stats[i]);
				stat.StatsList.stats[i].statIndex = i;
			}

			if (stats[0].Value == 0)
			{
				stats[0].ModifyFlat(45);
				stats[1].ModifyFlat(23);
				stats[2].ModifyFlat(0.2f);
				stats[3].ModifyFlat(0.2f);
				stats[4].ModifyFlat(9);
				stats[5].ModifyFlat(7);
				stats[7].ModifyFlat(0.75f);
				stats[10].ModifyFlat(9);
				stats[11].ModifyFlat(10);
				stats[12].ModifyFlat(1.25f);
				
				ModifyCharacterHealth(MaxHealth);
				ModifyCharacterMana(MaxMana);
			}

			LevelUp += UpdateLevelText;

			_inventory = GameManager.Instance.InventoryManager.playerInventory;
			_inventoryManager = GameManager.Instance.InventoryManager;

			_collider = GetComponent<Collider2D>();
		}

		protected override void Update()
		{
			base.Update();

			if (Input.anyKeyDown)
			{
				if (MoveRight.GetKeyDown())
				{
					ChangeCharacterDirection(false);
				}

				if (MoveLeft.GetKeyDown())
				{
					ChangeCharacterDirection(true);
				}
				
				if (_openChest)
				{
					if (Chest.GetKeyDown())
					{
						_inventoryManager.ToggleChest(_collidingChest);
					}
				}
			}
		}

		private void FixedUpdate()
		{
			MovePlayer();
		}

		public void Attack(Character target)
		{
			target.TakeDamage(this, AttackType.Normal_Attack, DamageType.Magical, 1, false);
		}

		public void LoadSaveData(PlayerSaveData saveData)
		{
			level = saveData.level;
			currentExp = saveData.currentExp;
			UpdateLevelText(this, level);

			healthRegenEffect = saveData.healthRegenEffect;
			manaRegenEffect = saveData.manaRegenEffect;

			stats = saveData.stats;
			
			ModifyCharacterHealth(saveData.currentHealth);
			ModifyCharacterMana(saveData.currentMana);
			
			GetComponent<StatusEffectController>().LoadSaveData(saveData);
		}

		private void MovePlayer()
		{
			var horizontalInput = InputManager.GetAxis(Axis.Horizontal);
			var verticalInput = InputManager.GetAxis(Axis.Vertical);

			if (horizontalInput != 0 || verticalInput != 0)
			{
				var moveSpeed = stats[12].Value;
				
				var velocity = new Vector2(horizontalInput * moveSpeed * 1.5f, verticalInput * moveSpeed * 1.5f);
                			
				Rigidbody2D.MovePosition(Rigidbody2D.position + velocity * Time.fixedDeltaTime);
			}
		}
		
		/// <summary>
		/// Default direction is right
		/// </summary>
		private void ChangeCharacterDirection(bool left)
		{
			SpriteRenderer.flipX = left;
			slashObject.transform.localPosition = new Vector3(left ? -1 : 1, 0);
		}
		
		private void UpdateLevelText(Character target, int lvl)
		{
			levelText.text = lvl.ToString();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Chest"))
			{
				if (_collidingChest != null)
				{
					if (Vector2.Distance(other.transform.position, transform.position) >
					    Vector2.Distance(_collidingChest.transform.position, transform.position)) return;
					
					_collidingChest.transform.GetChild(0).gameObject.SetActive(false);
				}
				
				_collidingChest = other.GetComponent<LootChest>();
				_collidingChest.transform.GetChild(0).gameObject.SetActive(true);
				_openChest = true;
			}
			else if (other.CompareTag("ItemDrop"))
			{
				var drop = other.GetComponent<ItemDrop>();
				
				drop.Count = _inventory.AddItems(drop.Item, drop.Count);

				//TODO display inventory full
			}
		}

		private void OnTriggerStay2D(Collider2D other)
		{ 
			if (_collidingChest == null) return;
			if (!other.CompareTag("Chest")) return;
			
			if (Vector2.Distance(other.transform.position, transform.position) <
			    Vector2.Distance(_collidingChest.transform.position, transform.position))
			{
				_collidingChest.transform.GetChild(0).gameObject.SetActive(false);

				var wasOpen = false;
				if (_inventoryManager.ChestOpen)
				{
					wasOpen = true;
					_inventoryManager.CloseChest();
				}
					
				_collidingChest = other.GetComponent<LootChest>();
				_collidingChest.transform.GetChild(0).gameObject.SetActive(true);
				_openChest = true;
				
				if (wasOpen) _inventoryManager.OpenChest(_collidingChest);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag("Chest"))
			{
				if (other.GetComponent<LootChest>() != _collidingChest) return;
				
				//This also closes chest
				if (_inventoryManager.ChestOpen) _inventoryManager.CloseVault();
				
				_collidingChest.transform.GetChild(0).gameObject.SetActive(false);
				_collidingChest = null;
				_openChest = false;
			}
		}
	}
}