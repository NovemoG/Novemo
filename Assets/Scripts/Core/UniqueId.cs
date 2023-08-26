using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
	public static class UniqueId
	{
		/// <summary>
		/// Generates a unique int value
		/// </summary>
		/// <param name="excludedIds">collection of ids to exclude</param>
		/// <param name="minValue">Inclusive</param>
		/// <param name="maxValue">Exclusive</param>
		public static int Generate(HashSet<int> excludedIds, int minValue, int maxValue)
		{
			var range = Enumerable.Range(minValue, maxValue).Where(i => !excludedIds.Contains(i));
			
			var index = Random.Range(minValue, maxValue - excludedIds.Count);
			
			return range.ElementAt(index);
		}
	}
}