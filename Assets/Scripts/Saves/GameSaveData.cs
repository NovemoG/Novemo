using System;
using System.Collections.Generic;

namespace Saves
{
	[Serializable]
	public class GameSaveData
	{
		public PlayerSaveData playerSaveData;
		
		public InventorySaveData inventorySaveData;
		public InventorySaveData vaultSaveData;
		public EquipmentSaveData equipmentSaveData;
		
		public List<RegionSaveData> regionSaveDatas;
	}
}