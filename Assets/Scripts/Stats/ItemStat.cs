using System;
using Enums;

namespace Stats
{
	[Serializable]
	public class ItemStat
	{
		public StatName statName;
		public float baseValue;
		public string bonusName;
		public float bonusValue;
	}
}