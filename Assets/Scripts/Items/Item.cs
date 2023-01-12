using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item", order = 1)]
    public class Item : ScriptableObject
    {
        public string itemName;
        public string itemDescription;
        
        public ItemType itemType;
        public Rarity itemRarity;

        public int stackLimit;
        public int sizeRows;
        public int sizeCols;
        
        public List<Tag> itemTags;

        //TODO calculate cost dynamically
        public int sellCost;
        public int buyCost;
        
        public virtual int ReferenceId => itemName.Length + itemDescription.Length + (int)itemRarity + (int)itemType + stackLimit;

        #region Equality override

        public static bool operator == (Item item1, Item item2)
        {
            return item1!.Equals(item2);
        }

        public static bool operator != (Item item1, Item item2)
        {
            return !(item1 == item2);
        }

        protected virtual bool Equals(Item other)
        {
            var equalTags = true;
            for (var i = 0; i < itemTags.Count; i++)
            {
                equalTags = other.itemTags[i] == itemTags[i];
            }
            
            return base.Equals(other) && ReferenceId == other.ReferenceId && equalTags;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Item)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ReferenceId);
        }

        #endregion
    }
}