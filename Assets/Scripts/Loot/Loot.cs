using System;
using Items;

namespace Loot
{
	[Serializable]
	public struct Loot
	{
		public Item item;
		public short minCount;
		public short maxCount;
		public short weight;
	}
}