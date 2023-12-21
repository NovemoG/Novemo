using System.Collections.Generic;
using Items;
using StatusEffects;
using UnityEngine;

namespace Core
{
	public class ItemDatabase : MonoBehaviour
	{
		public List<ItemData> itemsDatabase;
		public List<EffectData> effectsDatabase;

		private void Awake()
		{
			var id = 1;
			itemsDatabase = new List<ItemData>(Resources.LoadAll<ItemData>("Items"));
			for (int i = 0; i < itemsDatabase.Count; i++)
			{
				if (itemsDatabase[i].id == 0)
				{
					var none = itemsDatabase[i];
					
					itemsDatabase.RemoveAt(i);
					itemsDatabase.Insert(0, none);
					
					continue;
				}
				
				itemsDatabase[i].id = id;
				id++;
			}

			id = 1;
			effectsDatabase = new List<EffectData>(Resources.LoadAll<EffectData>("Effects"));
			for (int i = 0; i < effectsDatabase.Count; i++)
			{
				if (effectsDatabase[i].id == 0)
				{
					var none = effectsDatabase[i];
					
					effectsDatabase.RemoveAt(i);
					effectsDatabase.Insert(0, none);
					
					continue;
				}
				
				effectsDatabase[i].id = id;
				id++;
			}
		}
	}
}