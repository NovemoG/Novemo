using Characters.Player;
using Inventories;
using UnityEngine;
using static Enums.ActionCode;

namespace Managers
{
	public class PlayerManager : MonoBehaviour
	{
		public GameObject playerObject;
		public Player playerClass;

		private InventoryManager _inventoryManager;
		private Inventory _playerInventory;

		private void Awake()
		{
			_inventoryManager = GetComponent<InventoryManager>();
			_playerInventory = _inventoryManager.playerInventory;
		}

		private void Update()
		{
			if (Input.anyKeyDown)
			{
				if (Item1.GetKeyDown())
				{
					_playerInventory.allSlots[0].Item?.Use();
				}

				if (Item2.GetKeyDown())
				{
					_playerInventory.allSlots[1].Item?.Use();
				}

				if (Item3.GetKeyDown())
				{
					_playerInventory.allSlots[2].Item?.Use();
				}

				if (Item4.GetKeyDown())
				{
					_playerInventory.allSlots[3].Item?.Use();
				}

				if (Item5.GetKeyDown())
				{
					_playerInventory.allSlots[4].Item?.Use();
				}

				if (Item6.GetKeyDown())
				{
					_playerInventory.allSlots[5].Item?.Use();
				}

				if (Item7.GetKeyDown())
				{
					_playerInventory.allSlots[6].Item?.Use();
				}

				if (Item8.GetKeyDown())
				{
					_playerInventory.allSlots[7].Item?.Use();
				}

				if (Item9.GetKeyDown())
				{
					_playerInventory.allSlots[8].Item?.Use();
				}

				if (Item10.GetKeyDown())
				{
					_playerInventory.allSlots[9].Item?.Use();
				}

				if (Item11.GetKeyDown())
				{
					_playerInventory.allSlots[10].Item?.Use();
				}

				if (Item12.GetKeyDown())
				{
					_playerInventory.allSlots[11].Item?.Use();
				}
			}
		}
	}
}