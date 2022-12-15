using System;
using Items;

namespace Loot
{
	[Serializable]
	public struct Loot
	{
		public Item Item;
		public short minCount;
		public short maxCount;
		public short weight;
	}
}