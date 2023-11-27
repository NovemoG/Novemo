using System.Collections.Generic;
using System.Linq;
using Core;
using Items;
using UnityEngine;

namespace Loot
{
	[CreateAssetMenu(fileName = "Loot")]
	public class LootTableObject : ScriptableObject
	{
		[SerializeField] private List<Loot> possibleLoot;
		public List<GeneratedLoot> GeneratedLoot;
		
		private HashSet<int> _excludedIds;

		public void GenerateLoot()
		{
			GeneratedLoot = new List<GeneratedLoot>();
			_excludedIds = new HashSet<int>();
			var weight = 0;
			
			for (int i = 0; i < possibleLoot.Count; i++)
			{
				weight += possibleLoot[i].maxCount * possibleLoot[i].weight;
			}
			
			for (var i = 0; i < Metrics.ChestSize; i++)
			{
				foreach (var loot in possibleLoot.Where(l => l.item.itemName != "None"))
				{
					if (Random.Range(0, weight) > loot.weight * loot.maxCount) continue;

					var itemCount = loot.minCount > 0 ? loot.minCount : 1;
					
					for (int j = 1; j < loot.maxCount; j++)
					{
						itemCount += Random.Range(0, 100) < loot.weight * loot.maxCount / (itemCount - loot.minCount) ? 1 : 0;
					}
				
					GeneratedLoot.Add(new GeneratedLoot
					{
						Item = loot.item,
						Count = itemCount,
						SlotId = i
					});
					
					break;
				}
			}
			
			_excludedIds.Clear();
		}
		
		public void Reset()
		{
			var none = new Loot
			{
				item = Resources.Load<Item>("Items/None"),
				maxCount = 1
			};
			possibleLoot.Add(none);
		}
	}
}