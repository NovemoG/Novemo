using System;
using System.Collections.Generic;
using Loot;

namespace Saves
{
	[Serializable]
	public class ChestSaveData
	{
		public ChestSaveData(LootChest chest)
		{
			chestId = chest.id;
			inventory = chest.chestItems;
		}
		
		public int chestId;
		public List<GeneratedLoot> inventory;
	}
}