using System;
using Enums;
using Interfaces;
using Inventories;
using Items;
using Items.Equipment.Weapons;
using Loot;
using Managers;
using TMPro;
using UnityEngine;

namespace Characters.Player
{
	public class Player : Character, IAttack
	{
		public TextMeshProUGUI levelText;
		public GameObject weaponObject;
		public Transform slashObject;
		public Weapon weapon;

		public float playerSize = 1;

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
			
			CharacterStats["Health"].AddFlat(45);
			CharacterStats["Mana"].AddFlat(23);
			CharacterStats["Health Regen"].AddFlat(0.2f);
			CharacterStats["Mana Regen"].AddFlat(0.2f);
			CharacterStats["Physical Attack"].AddFlat(9);
			CharacterStats["Ability Power"].AddFlat(7);
			CharacterStats["Attack Speed"].AddFlat(0.75f);
			CharacterStats["Armor"].AddFlat(9);
			CharacterStats["Magic Resist"].AddFlat(10);
			CharacterStats["Movement Speed"].AddFlat(1);

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
				if (Input.GetKeyDown(KeyCode.A))
				{
					ChangeCharacterDirection(true);
				}

				if (Input.GetKeyDown(KeyCode.D))
				{
					ChangeCharacterDirection(false);
				}
				
				if (_openChest)
				{
					if (Input.GetKeyDown(KeyCode.C))
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
			target.TakeDamage(this, DamageType.Magical, 1, false);
		}

		private void MovePlayer()
		{
			var horizontalInput = Input.GetAxisRaw("Horizontal");
			var verticalInput = Input.GetAxisRaw("Vertical");

			if (horizontalInput != 0 || verticalInput != 0)
			{
				var moveSpeed = CharacterStats["Movement Speed"].Value;
				
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
		}

		private void OnTriggerStay2D(Collider2D other)
		{ 
			if (_collidingChest == null) return;
			
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