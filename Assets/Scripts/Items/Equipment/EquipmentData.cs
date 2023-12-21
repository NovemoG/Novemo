using System.Collections.Generic;
using Core;
using Enums;
using Stats;
using StatusEffects;
using UnityEngine;

namespace Items.Equipment
{
	[CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment/Equipment", order = 1)]
	public class EquipmentData : ItemData
	{
		[Header("Equipment")]
		public EquipSlotType equipSlotType;

		[Header("Bonuses")]
		[ListElementTitle("statName")] public List<ItemStat> stats;
		public List<EffectData> effects;
	}
}