using System.Collections.Generic;
using Core;
using Enums;
using Interfaces;
using Stats;
using StatusEffects;
using UnityEngine;

namespace Items.Equipment
{
	[CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment/Equipment")]
	public class Equipment : Item, IEquippable
	{
		public EquipSlotType equipSlotType;

		[ListElementTitle("statName")] public List<ItemStat> stats;
		public List<StatusEffect> effects;

		public override void GenerateTooltip()
		{
			ItemTooltip = string.Format(Templates.EquipmentTooltip, 
				Templates.FormatItemName(itemName, itemRarity), itemDescription, stackLimit);

			for (int i = 0; i < stats.Count; i++)
			{
				var statName = stats[i].statName.ToString().Replace('_', ' ');
				
				switch (stats[i].statName)
				{
					case StatName.Health:
					case StatName.Mana:
					case StatName.Health_Regen:
					case StatName.Mana_Regen:
					case StatName.Physical_Attack:
					case StatName.Ability_Power:
					case StatName.Armor:
					case StatName.Magic_Resist:
					case StatName.Armor_Penetration:
					case StatName.Magic_Resist_Penetration:
					case StatName.Life_Steal:
					case StatName.Ability_Vampirism:
						ItemTooltip += statName;
						
						if (stats[i].baseValue != 0)
						{
							var sign = stats[i].baseValue > 0 ? '+' : '-';
							ItemTooltip += $" {sign}{stats[i].baseValue}";
						}
						if (stats[i].bonusValue != 0)
						{
							var sign = stats[i].bonusValue > 0 ? '+' : '-';
							ItemTooltip += $" {sign}{stats[i].bonusValue}%";
						}
						
						ItemTooltip += "\n";
						break;
					case StatName.Lethal_Damage:
					case StatName.Attack_Speed:
					case StatName.Crit_Rate:
					case StatName.Crit_Bonus:
					case StatName.Movement_Speed:
					case StatName.Cooldown_Reduction:
					case StatName.Luck:
					case StatName.Exp_Bonus:
					case StatName.Counter_Chance:
					case StatName.Double_Attack_Chance:
						ItemTooltip += string.Format($"{statName} +{stats[i].bonusValue}%\n");
						break;
				}
			}
			
			for (int i = 0; i < effects.Count; i++)
			{
				ItemTooltip += $"{effects[i].EffectName}\n{effects[i].FormattedDescription}\n";
			}

			ItemTooltip += $"<color=#282828><size=20><i>{equipSlotType}</i></size></color>";

			ItemTooltip += string.Format(Templates.ItemTypeTooltip, itemType);
		}

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

		protected override bool Equals(Item other)
		{
			var equipment = other as Equipment;

			return true;
		}
	}
}