using System;
using Items;
using UnityEngine;

namespace Saves
{
	[Serializable]
	public class ItemDropData
	{
		public ItemDropData(ItemDrop itemDrop)
		{
			position = itemDrop.transform.localPosition;
			item = itemDrop.Item;
			count = itemDrop.Count;
		}
		
		public Vector3 position;
		public Item item;
		public int count;
	}
}