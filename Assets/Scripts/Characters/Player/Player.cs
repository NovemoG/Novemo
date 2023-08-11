using System;
using Enums;
using Interfaces;
using Inventories;
using Items;
using Managers;
using TMPro;
using UnityEngine;

namespace Characters.Player
{
	public class Player : Character, IAttack
	{
		public TextMeshProUGUI levelText;

		private Inventory _inventory;
		
		/// <summary>
		/// Gives information about item added to inventory
		/// </summary>
		public event Action<Item> ItemCollected;
		public void InvokeItemCollected(Item item)
		{
			ItemCollected?.Invoke(item);
		}

		protected override void Awake()
		{
			base.Awake();
			
			stats[0].AddFlat(45);
			stats[1].AddFlat(23);
			stats[2].AddFlat(9);
			stats[3].AddFlat(7);
			stats[5].AddFlat(0.75f);
			stats[8].AddFlat(9);
			stats[9].AddFlat(10);
			stats[10].AddFlat(1);

			LevelUp += UpdateLevelText;

			_inventory = GameManager.Instance.InventoryManager.playerInventory;
		}

		protected override void Update()
		{
			base.Update();

			if (Input.anyKeyDown)
			{
				//TODO check key binds instead of keys
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					_inventory.AllSlots[0].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					_inventory.AllSlots[1].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					_inventory.AllSlots[2].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					_inventory.AllSlots[3].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					_inventory.AllSlots[4].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					_inventory.AllSlots[5].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha7))
				{
					_inventory.AllSlots[6].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha8))
				{
					_inventory.AllSlots[7].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha9))
				{
					_inventory.AllSlots[8].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha0))
				{
					_inventory.AllSlots[9].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Minus))
				{
					_inventory.AllSlots[10].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Equals))
				{
					//TODO static classes for item uses
					_inventory.AllSlots[11].Peek.Use();
				}
			}
		}

		public void Attack(Character target)
		{
			target.TakeDamage(this, DamageType.Magical, 1, false);
		}
		
		private void UpdateLevelText(Character target, int lvl)
		{
			levelText.text = lvl.ToString();
		}
	}
}