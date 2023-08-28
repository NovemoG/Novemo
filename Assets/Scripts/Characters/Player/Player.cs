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
			var moveSpeed = CharacterStats["Movement Speed"].Value;
			var horizontalInput = Input.GetAxisRaw("Horizontal");
			var verticalInput = Input.GetAxisRaw("Vertical");

			if (horizontalInput != 0 || verticalInput != 0)
			{
				var velocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
                			
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
			_openChest = true;
			
			if (other.gameObject.CompareTag("Chest"))
			{
				_collidingChest = other.gameObject.GetComponent<LootChest>();
			}

			//TODO activate border
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			_openChest = false;
			
			if (other.gameObject.CompareTag("Chest"))
			{
				_collidingChest = other.gameObject.GetComponent<LootChest>();
			}
			
			//TODO deactivate border
		}
	}
}