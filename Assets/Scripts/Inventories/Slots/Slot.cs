using System.Collections.Generic;
using Interfaces;
using Items;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories.Slots
{
    public class Slot : MonoBehaviour
    {
        private List<Item> _items;

        public Item Peek => !IsEmpty ? _items[0] : null;
        
        public int Count => _items.Count;
        
        public bool IsEmpty => Count == 0;
        public bool IsFull => !IsEmpty && _items[0].stackLimit == Count;

        public Image slotIcon;
        public TextMeshProUGUI stackText;
        
        public GameObject borderObject;
        public GameObject backgroundObject;

        private void Awake()
        {
            _items = new List<Item>();
            GetComponent<Toggle>().group = GameManager.Instance.InventoryManager.inventoryToggleGroup;
        }

        public bool AddItem(Item item)
        {
            if (IsFull) return false;
            
            if (IsEmpty || Peek == item)
            {
                _items.Add(item);
            }

            if (Count == 1)
            {
                slotIcon.gameObject.SetActive(true);
                slotIcon.sprite = item.itemIcon;
            }
            
            stackText.text = Count > 1 ? "" : Count.ToString();

            return true;
        }

        public bool RemoveItem()
        {
            if (IsEmpty) return false;
            
            _items.RemoveAt(0);

            if (IsEmpty)
            {
                slotIcon.gameObject.SetActive(false);
            }
            
            stackText.text = Count <= 1 ? "" : Count.ToString();
            
            return true;
        }

        public void ToggleBorder()
        {
            borderObject.SetActive(!borderObject.activeSelf);
        }
    }
}