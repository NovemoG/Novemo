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
        
        public bool IsEmpty => _items.Count == 0;
        public bool IsFull => !IsEmpty && _items[0].stackLimit == _items.Count;

        public Image slotIcon;
        public TextMeshProUGUI stackText;
        
        public GameObject borderObject;
        public GameObject backgroundObject;
        
        public List<Item> Items => _items;
        private List<Item> _items;

        public Toggle ToggleComponent { get; private set; }

        private void Awake()
        {
            _items = new List<Item>();
            ToggleComponent = GetComponent<Toggle>();
            ToggleComponent.group = GameManager.Instance.InventoryManager.inventoryToggleGroup;
        }

        public bool AddItem(Item item)
        {
            if (IsFull) return false;

            if (!IsEmpty && Peek != item)
            {
                return false;
            }

            _items.Add(item);

            switch (_items.Count)
            {
                case 1:
                    slotIcon.gameObject.SetActive(true);
                    slotIcon.sprite = item.itemIcon;
                    break;
                case > 1:
                    stackText.gameObject.SetActive(true);
                    stackText.text = _items.Count.ToString();
                    break;
            }

            return true;
        }

        public List<Item> AddItems(List<Item> items)
        {
            items = new List<Item>(items);
            
            for (int i = 0; i < items.Count; i++)
            {
                if (!AddItem(items[0])) break;
                items.RemoveAt(0);
            }

            return items;
        }

        public bool RemoveItem()
        {
            if (IsEmpty) return false;
            
            _items.RemoveAt(0);

            if (IsEmpty)
            {
                slotIcon.gameObject.SetActive(false);
            }
            
            stackText.text = _items.Count.ToString();
            
            if (_items.Count <= 1)
            {
                stackText.gameObject.SetActive(false);
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

        /// <summary>
        /// Removes all items from given slot
        /// </summary>
        public void ClearSlot()
        {
            _items.Clear();
            stackText.text = "";
            stackText.gameObject.SetActive(false);
            slotIcon.gameObject.SetActive(false);
        }

        public void ToggleBorder()
        {
            borderObject.SetActive(ToggleComponent.isOn);
        }
    }
}