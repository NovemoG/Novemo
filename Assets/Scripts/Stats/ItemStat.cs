using System;
using Core;
using Enums;

namespace Stats
{
	[Serializable]
	public struct ItemStat
	{
		public StatName statName;
		public float baseValue;
		public float bonusValue;

		#region Equality override

		public static bool operator == (ItemStat stat1, ItemStat stat2)
		{
			return stat1.Equals(stat2);
		}

		public static bool operator != (ItemStat stat1, ItemStat stat2)
		{
			return !(stat1 == stat2);
		}
		
		public bool Equals(ItemStat other)
		{
			return statName == other.statName && baseValue.CompareWith(other.baseValue) && bonusValue.CompareWith(other.bonusValue);
		}

		public override bool Equals(object obj)
		{
			return obj is ItemStat other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine((int)statName, baseValue, bonusValue);
		}

		#endregion
	}
}