using System.Collections.Generic;
using Items;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories.Slots
{
    public class Slot : MonoBehaviour
    {
        public Item Peek => !IsEmpty ? _items[0] : null;
        
        public int ItemCount => _items.Count;
        
        public bool IsEmpty => ItemCount == 0;
        public bool IsFull => !IsEmpty && _items[0].stackLimit == ItemCount;

        public Image slotIcon;
        public TextMeshProUGUI stackText;
        
        public GameObject borderObject;
        public GameObject backgroundObject;

        private Toggle _toggleComponent;
        
        private List<Item> _items;

        private void Awake()
        {
            _items = new List<Item>();
            _toggleComponent = GetComponent<Toggle>();
            _toggleComponent.group = GameManager.Instance.InventoryManager.inventoryToggleGroup;
        }

        public bool AddItem(Item item)
        {
            if (IsFull) return false;

            if (!IsEmpty && Peek != item)
            {
                return false;
            }

            _items.Add(item);

            if (ItemCount == 1)
            {
                slotIcon.gameObject.SetActive(true);
                slotIcon.sprite = item.itemIcon;
            }

            if (ItemCount > 1)
            {
                stackText.gameObject.SetActive(true);
                stackText.text = ItemCount.ToString();
            }

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
            
            stackText.text = ItemCount.ToString();
            
            if (ItemCount <= 1)
            {
                stackText.gameObject.SetActive(false);
            }
            
            return true;
        }

        /// <summary>
        /// Removes all items from given slot
        /// </summary>
        public void ClearSlot()
        {
            _items.Clear();
            stackText.text = "";
            stackText.gameObject.SetActive(true);
        }

        public void ToggleBorder()
        {
            borderObject.SetActive(_toggleComponent.isOn);
        }
    }
}