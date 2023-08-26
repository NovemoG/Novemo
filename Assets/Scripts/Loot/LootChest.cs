using UnityEngine;

namespace Loot
{
	public class LootChest : MonoBehaviour
	{
		public LootTableObject lootTable;
		
		protected void Awake()
		{
			lootTable.GenerateLoot();
		}
	}
}