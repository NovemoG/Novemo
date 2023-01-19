using System.Collections.Generic;
using System.Linq;
using Items;
using TMPro;

namespace Inventory.Slots
{
	public struct SlotGroup
	{
		public readonly List<Slot> Slots;
		public readonly Slot PivotSlot;
		public readonly TextMeshProUGUI stackTextSlot;

		/// <summary>
		/// Container for items bigger than 1x1
		/// </summary>
		/// <param name="xCoordinate">first slot x position in inventory</param>
		/// <param name="yCoordinate">first slot y position in inventory</param>
		/// <param name="slots">slots collection</param>
		public SlotGroup(List<Slot> slots)
		{
			Slots = slots;
			PivotSlot = Slots[0];
			stackTextSlot = Slots.Last().stackText;
		}

		public bool AddItem(Item item)
		{
			if (!PivotSlot.AddItem(item)) return false;
			
			CreateIcon();
			return true;

		}

		private void CreateIcon()
		{
			
		}
	}
}