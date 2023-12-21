using Core.Converters.Items;
using Enums;
using FullSerializer;
using UnityEngine;

namespace Items
{
    [fsObject(Processor = typeof(ItemDataProcessor), Converter = typeof(ItemDataConverter))]
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item", order = 1)]
    public class ItemData : ScriptableObject
    {
        public int id = -1;
        
        [Header("Item details")]
        public string itemName;
        public string itemDescription;
        public Sprite itemIcon;
        public ItemType itemType;
        public Rarity itemRarity;
        public int stackLimit = 1;
        public bool questItem;
        
        [Header("Item value")]
        public int baseSellCost;
        public int baseBuyCost;
    }
}