using Enums;
using Interfaces;
using UnityEngine;

namespace Items.Equipment
{
	[CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment/Equipment")]
	public class Equipment : Item, IEquippable
	{
		public EquipSlotType equipSlotType;

		public override bool Use()
		{
			return Equip();
		}
		
		public virtual bool Equip()
		{
			//TODO equip logic

			return true;
		}

		public virtual bool Unequip()
		{


			return true;
		}
	}
}