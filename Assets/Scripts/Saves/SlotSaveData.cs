using System;
using Inventories.Slots;
using Items;

namespace Saves
{
	[Serializable]
	public class SlotSaveData
	{
		public SlotSaveData(Slot slot)
		{
			item = slot.Item;
			count = slot.ItemCount;
		}
		
		public Item item;
		public int count;
	}
}