using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
	public class LootChest : MonoBehaviour
	{
		public int id;
		
		[SerializeField] private LootTableObject lootTable;
		public List<GeneratedLoot> chestItems;
		
		protected void Awake()
		{
			if (lootTable == null) return;
			
			lootTable.GenerateLoot();
			chestItems = lootTable.generatedLoot;
		}
	}
}