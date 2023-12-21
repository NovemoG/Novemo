using System;
using System.Collections.Generic;
using Core;
using Enums;
using Interfaces;
using Stats;
using StatusEffects;

namespace Items.Equipment
{
	[Serializable]
	public class Equipment : Item, IEquippable
	{
		public Equipment(ItemData itemData) : base(itemData)
		{
			stats = new List<ItemStat>();
			effects = new List<StatusEffect>();
		}
		
		public Equipment(EquipmentData equipmentData) : base(equipmentData)
		{
			equipSlotType = equipmentData.equipSlotType;
			stats = equipmentData.stats;
			effects = new List<StatusEffect>();

			foreach (var effect in equipmentData.effects)
			{
				effects.Add(Create.EffectInstance(effect));
			}
		}
		
		public EquipSlotType equipSlotType;
		
		public List<ItemStat> stats;
		public List<StatusEffect> effects;

		public override void GenerateTooltip()
		{
			ItemTooltip = string.Format(Templates.EquipmentTooltip, 
				Templates.FormatItemName(Name, Rarity), Description, StackLimit);

			if (stats.Count != 0) ItemTooltip += Templates.NewLine;
			for (int i = 0; i < stats.Count; i++)
			{
				var statName = stats[i].statName.ToString().Replace('_', ' ');
				
				//TODO get colored tooltip from templates
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
						ItemTooltip += $"<color=#4E4E4EC8><size=20>{statName}";
						
						if (stats[i].baseValue != 0)
						{
							var sign = stats[i].baseValue > 0 ? '+' : '-';
							ItemTooltip += $" {sign}{stats[i].baseValue}";
						}
						if (stats[i].bonusValue != 0)
						{
							var sign = stats[i].bonusValue > 0 ? '+' : '-';
							ItemTooltip += $" {sign}{stats[i].bonusValue * 100}%";
						}
						
						ItemTooltip += "</size></color>\n";
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
						ItemTooltip += string.Format($"{statName} +{stats[i].bonusValue * 100}%\n");
						break;
				}
			}
			
			if (effects.Count != 0) ItemTooltip += Templates.NewLine;
			for (int i = 0; i < effects.Count; i++)
			{
				ItemTooltip += $"<color=#282828><size=20><b>{effects[i].EffectName}</b>\n{effects[i].FormattedDescription}</size></color>\n";
			}

			ItemTooltip += $"{Templates.NewLine}<color=#282828><size=20><i>{equipSlotType}</i></size></color>\n";

			ItemTooltip += string.Format(Templates.ItemTypeTooltip, Type);
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
			if (other is not Equipment equipment) return false;

			if (equipSlotType != equipment.equipSlotType) return false;
			if (effects.Count != equipment.effects.Count) return false;
			if (stats.Count != equipment.stats.Count) return false;
			
			for (int i = 0; i < effects.Count; i++)
			{
				if (effects[i].EffectData.id != equipment.effects[i].EffectData.id) return false;
			}
			for (int i = 0; i < stats.Count; i++)
			{
				if (stats[i] != equipment.stats[i]) return false;
			}
			
			return true;
		}
	}
}