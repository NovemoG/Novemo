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
		public List<GeneratedLoot> generatedLoot;

		public void GenerateLoot()
		{
			generatedLoot = new List<GeneratedLoot>();
			var weight = 0;
			
			for (int i = 0; i < possibleLoot.Count; i++)
			{
				weight += possibleLoot[i].maxCount * possibleLoot[i].weight;
			}
			
			for (var i = 0; i < Metrics.ChestSize; i++)
			{
				foreach (var loot in possibleLoot.Where(l => l.item.Name != "None"))
				{
					if (Random.Range(0, weight) > loot.weight * loot.maxCount) continue;

					var itemCount = loot.minCount > 0 ? loot.minCount : 1;
					
					for (int j = 1; j < loot.maxCount; j++)
					{
						itemCount += Random.Range(0, 100) < loot.weight * loot.maxCount / (itemCount - loot.minCount) ? 1 : 0;
					}
					
					loot.item.GenerateTooltip();
				
					generatedLoot.Add(new GeneratedLoot
					{
						item = loot.item,
						count = itemCount,
						slotId = i
					});
					
					break;
				}
			}
		}
		
		public void Reset()
		{
			var none = new Loot
			{
				item = new Item(Resources.Load<ItemData>("Items/None")),
				maxCount = 1
			};
			possibleLoot.Add(none);
		}
	}
}