using System.Collections.Generic;
using Core;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories.Slots
{
    public class Slot : MonoBehaviour
    {
        //group in which this slot is allocated if there is an item bigger than 1x1
        public SlotGroup SlotGroup;
        public Coordinates2D SlotCoordinates;

        public List<Item> Items { get; private set; }
        
        public bool IsEmpty => Items.Count == 0;
        public bool IsFull => !IsEmpty && Items[0].stackLimit == Items.Count;
        public bool IsInGroup => SlotGroup.Slots is not null && SlotGroup.Slots.Count != 0;

        public Image slotIcon;
        public TextMeshProUGUI stackText;
        public GameObject backgroundObject;
        
        private void Awake()
        {
            Items = new List<Item>();
        }

        public bool AddItem(Item item)
        {
            if ((IsEmpty || Items[0] == item) && !IsFull)
            {
                Items.Add(item);
                return true;
            }

            return false;

        }

        public List<Item> AddItems(List<Item> itemsAdded)
        {
            while (itemsAdded.Count > 0 && AddItem(itemsAdded[0]))
            {
                itemsAdded.RemoveAt(0);
            }
            
            return itemsAdded;
        }

        public bool RemoveItem()
        {
            if (IsEmpty) return false;
            
            Items.RemoveAt(0);
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