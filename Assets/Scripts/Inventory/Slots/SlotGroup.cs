using System.Collections.Generic;
using TMPro;

namespace Inventory.Slots
{
	public struct SlotGroup
	{
		public List<Slot> Slots;
		
		//top left slot is the pivot that contains all item data
		public Slot PivotSlot;
		public TextMeshProUGUI stackTextSlot;

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
			stackTextSlot = Slots[Slots.Count].stackText;
		}
	}
}