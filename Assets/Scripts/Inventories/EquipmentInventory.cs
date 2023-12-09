using System.Collections.Generic;
using Inventories.Slots;
using Managers;
using UnityEngine;

namespace Inventories
{
	public class EquipmentInventory : MonoBehaviour
	{
		private InventoryManager _inventoryManager;

		public List<EquipmentSlot> slots;

		private void Awake()
		{
			_inventoryManager = GameManager.Instance.InventoryManager;

			foreach (Transform child in transform)
			{
				slots.Add(child.GetComponent<EquipmentSlot>());
			}
		}
	}
}