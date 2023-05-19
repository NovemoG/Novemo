using System;
using Characters;
using Items.Equipment;
using UnityEngine;

namespace Managers
{
	public class EquipmentManager : MonoBehaviour
	{
		public event Action<Character, Equipment> ItemEquipped;
		public event Action<Character, Equipment> ItemUnequipped;

		public void InvokeItemEquipped(Character character, Equipment item)
		{
			ItemEquipped?.Invoke(character, item);
		}

		public void InvokeItemUnequipped(Character character, Equipment item)
		{
			ItemUnequipped?.Invoke(character, item);
		}

		private void Awake()
		{
			ItemEquipped += OnItemEquipped;
			ItemUnequipped += OnItemUnequipped;
		}

		private void OnItemEquipped(Character character, Equipment item)
		{
			
		}

		private void OnItemUnequipped(Character character, Equipment item)
		{
			
		}
	}
}