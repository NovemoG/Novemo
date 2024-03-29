using System.Collections.Generic;
using Core.Patterns;
using UnityEngine;

namespace Stats
{
	[CreateAssetMenu(fileName = "Stats List")]
	public class StatsObject : ScriptableObject
	{
		public List<Stat> stats;

		private void Reset()
		{
			stats = StatsListPattern.StatsPattern;
		}
	}
}