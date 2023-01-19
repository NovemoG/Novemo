using System.Collections.Generic;
using Core;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Slots
{
    public class Slot : MonoBehaviour
    {
        //group in which this slot is allocated if there is an item bigger than 1x1
        public SlotGroup slotGroup;
        public Coordinates2D slotCoordinates;

        protected List<Item> items;
        
        public bool IsEmpty => PeekItem is null;
        public bool IsFull => PeekItem.stackLimit == items.Count;
        public bool IsInGroup => slotGroup.Slots is not null;
        public Item PeekItem => !IsEmpty ? items[0] : null;

        public Image slotIcon;
        public TextMeshProUGUI stackText;
        
        private void Awake()
        {
            items = new List<Item>();
        }

        public bool AddItem(Item item)
        {
            if (PeekItem != item || PeekItem.stackLimit == items.Count) return false;
            
            items.Add(item);
            slotIcon.enabled = true;
            slotIcon.sprite = item.itemIcon;
            return true;
        }

        public List<Item> AddItems(List<Item> itemsAdded)
        {
            while (AddItem(items[0]))
            {
                itemsAdded.RemoveAt(0);
            }
            
            return itemsAdded;
        }

        public bool RemoveItem()
        {
            if (IsEmpty) return false;
            
            items.RemoveAt(0);
            if (IsEmpty)
            {
                stackText.enabled = false;
                slotIcon.sprite = null;
                slotIcon.enabled = false;
            }
            return true;
        }

        public int RemoveItems(int count)
        {
            while (RemoveItem())
            {
                count--;
            }

            return count;
        }
    }
}