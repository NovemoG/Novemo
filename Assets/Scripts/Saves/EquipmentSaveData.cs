using System;
using System.Collections.Generic;
using Enums;
using Inventories;
using Inventories.Slots;
using Items.Equipment;

namespace Saves
{
	[Serializable]
	public class EquipmentSaveData
	{
		public EquipmentSaveData(EquipmentInventory inventory)
		{
			eqSlotSaveDatas = new List<EqSlotSaveData>();

			for (int i = 0; i < inventory.slots.Count; i++)
			{
				eqSlotSaveDatas.Add(new EqSlotSaveData(inventory.slots[i]));
			}
		}

		public List<EqSlotSaveData> eqSlotSaveDatas;
	}
	
	[Serializable]
	public class EqSlotSaveData
	{
		public EqSlotSaveData(EquipmentSlot slot)
		{
			equipment = slot.Item as Equipment;
			slotType = slot.equipSlotType;
		}
		
		public Equipment equipment;
		public EquipSlotType slotType;
	}
}