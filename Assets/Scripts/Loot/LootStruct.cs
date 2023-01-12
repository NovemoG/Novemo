using System;
using Items;

namespace Loot
{
	[Serializable]
	public struct LootStruct
	{
		public Item Item;
		public short minCount;
		public short maxCount;
		public short weight;
		public LootTableObject lootTable;
	}
}