using System;
using System.Collections.Generic;
using Inventories;

namespace Saves
{
	[Serializable]
	public class InventorySaveData
	{
		public InventorySaveData(Inventory inventory)
		{
			slotsSaveData = new List<SlotSaveData>();
			
			for (int i = 0; i < inventory.allSlots.Count; i++)
			{
				slotsSaveData.Add(new SlotSaveData(inventory.allSlots[i]));
			}
		}
		
		public List<SlotSaveData> slotsSaveData;
	}
}