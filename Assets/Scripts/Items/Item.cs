using System;
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

        //TODO create algorithm for this
        public int ReferenceId => itemName.Length + itemDescription.Length + (int)itemRarity + (int)itemType;

        #region Equality override

        public static bool operator == (Item item1, Item item2)
        {
            return item1!.Equals(item2);
        }

        public static bool operator != (Item item1, Item item2)
        {
            return !(item1 == item2);
        }

        private bool Equals(Item other)
        {
            return base.Equals(other) && ReferenceId == other.ReferenceId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == this.GetType() && Equals((Item)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ReferenceId);
        }

        #endregion
    }
}