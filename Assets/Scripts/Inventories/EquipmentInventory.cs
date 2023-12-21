using System.Collections.Generic;
using Inventories.Slots;
using Managers;
using Saves;
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
			
			if (slots.Count == 0)
			{
				slots = new List<EquipmentSlot>();
                
				foreach (Transform child in transform)
				{
					slots.Add(child.GetComponent<EquipmentSlot>());
				}
			}
		}

		public void LoadSaveData(EquipmentSaveData saveData)
		{
			slots = new List<EquipmentSlot>();
			
			var i = 0;
			foreach (Transform child in transform)
			{
				var slotComponent = child.GetComponent<EquipmentSlot>();

				var slotData = saveData.eqSlotSaveDatas[i];
				if (slotData.equipment != null && slotData.slotType == slotComponent.equipSlotType)
				{
					slotComponent.AddItem(slotData.equipment, false);
				}
				
				slots.Add(slotComponent);
				i++;
			}
		}
	}
}