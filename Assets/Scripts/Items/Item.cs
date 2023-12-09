using System;
using Core;
using Enums;
using Interfaces;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item", order = 1)]
    public class Item : ScriptableObject, IUsable
    {
        public int id;
        
        public string itemName;
        public string itemDescription;
        public Sprite itemIcon;
        //public GeneratedImage itemIcon;

        public ItemType itemType;
        public Rarity itemRarity;

        public int stackLimit;

        //public List<Tag> itemTags;

        //TODO calculate cost dynamically
        public int baseSellCost;
        public int baseBuyCost;
        public virtual int SellCost => baseSellCost;
        public virtual int BuyCost => baseBuyCost;

        [NonSerialized] public string ItemTooltip;
        
        public virtual void GenerateTooltip()
        {
            ItemTooltip = string.Format(Templates.ItemTooltip,
                Templates.FormatItemName(itemName, itemRarity), itemDescription, stackLimit, itemType);
        }

        public virtual bool Use()
        {
            return false;
        }

        protected virtual void OnEnable()
        {
            GenerateTooltip();
        }

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
            /*var equalTags = true;

            if (itemTags != null)
            {
                for (var i = 0; i < itemTags.Count; i++)
                {
                    equalTags = other.itemTags[i] == itemTags[i];
                    
                    if (!equalTags) break;
                }
            }*/
                        
            return base.Equals(other) && id == other.id;// && equalTags;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == this.GetType() && Equals((Item)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), id);
        }

        #endregion
    }
}