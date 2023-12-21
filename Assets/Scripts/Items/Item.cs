using System;
using Core;
using Core.Converters.Items;
using Enums;
using FullSerializer;
using Interfaces;
using UnityEngine;

namespace Items
{
	[Serializable]
	[fsObject(Processor = typeof(ItemProcessor))]
	public class Item : IUsable
	{
		public Item(ItemData itemData)
		{
			this.itemData = itemData;
		}

		[SerializeField] protected ItemData itemData;
		[fsIgnore] public int Id => itemData.id;
		[fsIgnore] public string Name => itemData.itemName;
		[fsIgnore] public string Description => itemData.itemDescription; 
		[fsIgnore] public Sprite Icon => itemData.itemIcon;
		[fsIgnore] public ItemType Type => itemData.itemType;
		[fsIgnore] public Rarity Rarity => itemData.itemRarity;
		[fsIgnore] public int StackLimit => itemData.stackLimit;
		[fsIgnore] public bool QuestItem => itemData.questItem;
		
		[fsIgnore] public string ItemTooltip { get; protected set; }
		public virtual void GenerateTooltip() //TODO compute power vs memory
		{
			ItemTooltip = string.Format(Templates.ItemTooltip,
				Templates.FormatItemName(Name, Rarity), Description, StackLimit, Type);
		}

		public virtual bool Use()
		{
			return false;
		}
		
		//TODO calculate cost dynamically
		[fsIgnore] public virtual int SellCost => itemData.baseSellCost;
		[fsIgnore] public virtual int BuyCost => itemData.baseBuyCost;

		#region Equality override

		public static bool operator == (Item item1, Item item2)
		{
			if (ReferenceEquals(null, item1)) return true;
            
			return item1.Equals(item2);
		}

		public static bool operator != (Item item1, Item item2)
		{
			return !(item1 == item2);
		}
		
		protected virtual bool Equals(Item other)
		{
			if (ReferenceEquals(null, other)) return false;
			
			return Id == other.Id && QuestItem == other.QuestItem;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Item)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, QuestItem);
		}
		
		#endregion
	}
}