using System;
using Characters;
using Enums;
using Items.Equipment;
using UnityEngine;

namespace Managers
{
	public class EquipmentManager : MonoBehaviour
	{
		public event Action<Character, Equipment> ItemEquipped;
		public event Action<Character, Equipment> ItemUnequipped;

		public void EquipCharacterItem(Character character, Equipment item)
		{
			ItemEquipped?.Invoke(character, item);
		}

		public void UnequipCharacterItem(Character character, Equipment item)
		{
			ItemUnequipped?.Invoke(character, item);
		}

		private void Awake()
		{
			ItemEquipped += AddItemStats;
			ItemEquipped += ApplyItemEffects;
			ItemUnequipped += RemoveItemStats;
			ItemUnequipped += RemoveItemEffects;
		}

		private void AddItemStats(Character character, Equipment item)
		{
			for (int i = 0; i < item.stats.Count; i++)
			{
				var stat = item.stats[i];
				
				character.Stats[(int)stat.statName].ModifyFlat(stat.baseValue);
				character.Stats[(int)stat.statName].ModifyBonus(stat.bonusValue);
				
				if (stat.statName == StatName.Health) character.UpdateHealthData();
				else if (stat.statName == StatName.Mana) character.UpdateManaData();
			}
		}

		private void RemoveItemStats(Character character, Equipment item)
		{
			for (int i = 0; i < item.stats.Count; i++)
			{
				var stat = item.stats[i];
				
				character.Stats[(int)stat.statName].ModifyFlat(-stat.baseValue);
				character.Stats[(int)stat.statName].ModifyBonus(-stat.bonusValue);
				
				if (stat.statName == StatName.Health) character.UpdateHealthData();
				else if (stat.statName == StatName.Mana) character.UpdateManaData();
			}
		}
		
		private void ApplyItemEffects(Character character, Equipment item)
		{
			for (int i = 0; i < item.effects.Count; i++)
			{
				var effect = item.effects[i];
				
				character.ApplyEffect(effect);
			}
		}

		private void RemoveItemEffects(Character character, Equipment item)
		{
			for (int i = 0; i < item.effects.Count; i++)
			{
				character.RemoveEffect(item.effects[i].Id);
			}
		}
	}
}