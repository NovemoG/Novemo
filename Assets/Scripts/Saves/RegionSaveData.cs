using System;
using System.Collections.Generic;

namespace Saves
{
	[Serializable]
	public class RegionSaveData
	{
		public int id;
		
		public List<ChestSaveData> chestSaveDatas;
		public List<ItemDropData> itemDropDatas;
	}
}