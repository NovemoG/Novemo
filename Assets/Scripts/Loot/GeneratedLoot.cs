using System;
using Items;

namespace Loot
{
	[Serializable]
	public struct GeneratedLoot
	{
		public Item item;
		public int count;
		public int slotId;
	}
}