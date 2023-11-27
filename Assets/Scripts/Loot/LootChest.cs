using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
	public class LootChest : MonoBehaviour
	{
		[SerializeField] private LootTableObject lootTable;
		public List<GeneratedLoot> ChestItems;
		
		protected void Awake()
		{
			lootTable.GenerateLoot();
			ChestItems = lootTable.GeneratedLoot;
		}
	}
}