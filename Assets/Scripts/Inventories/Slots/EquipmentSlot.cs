using Enums;
using Items.Equipment;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventories.Slots
{
	[RequireComponent(typeof(TooltipHandler))]
	public class EquipmentSlot : MonoBehaviour
	{
		public Equipment equippedItem;
		public EquipSlotType equipSlotType;

		public Button ButtonComponent { get; private set; }
	}
}