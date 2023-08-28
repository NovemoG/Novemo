using Characters.Player;
using Inventories;
using UnityEngine;

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
				//TODO check key binds instead of keys
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					_playerInventory.AllSlots[0].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					_playerInventory.AllSlots[1].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					_playerInventory.AllSlots[2].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					_playerInventory.AllSlots[3].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					_playerInventory.AllSlots[4].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					_playerInventory.AllSlots[5].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha7))
				{
					_playerInventory.AllSlots[6].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha8))
				{
					_playerInventory.AllSlots[7].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha9))
				{
					_playerInventory.AllSlots[8].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Alpha0))
				{
					_playerInventory.AllSlots[9].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Minus))
				{
					_playerInventory.AllSlots[10].Peek.Use();
				}

				if (Input.GetKeyDown(KeyCode.Equals))
				{
					_playerInventory.AllSlots[11].Peek.Use();
				}
			}
		}
	}
}