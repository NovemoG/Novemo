using Interfaces;
using UnityEngine;

namespace Items.Consumables
{
	[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Consumables/Consumable")]
	public class Consumable : ItemData
	{
		public int useCount;
		/// <summary>
		/// In seconds, if unchanged it will only regenerate in safe zones
		/// </summary>
		public int useCooldown;
	}
}