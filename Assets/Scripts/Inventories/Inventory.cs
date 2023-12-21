using System.Collections.Generic;
using Core;
using Inventories.Slots;
using Items;
using Managers;
using Saves;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories
{
    public class Inventory : MonoBehaviour
    {
        private InventoryManager _inventoryManager;

        public GameObject inventoryBackground;
        public int rows, cols, slotCount, freeSlots;
        
        [HideInInspector] public List<Slot> allSlots;

        private void Awake()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;

            if (allSlots.Count == 0)
            {
                allSlots = new List<Slot>();
                
                foreach (Transform child in transform)
                {
                    allSlots.Add(child.GetComponent<Slot>());
                    freeSlots++;
                }
            }
        }

        private void Start()
        {
            if (gameObject.name == "Inventory")
            {
                allSlots[0].GetComponent<Toggle>().isOn = true;
            }
            else
            {
                transform.parent.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            
        }

        /// <summary>
        /// Tries to add one item to inventory
        /// </summary>
        /// <returns>Whether the provided item was successfully added</returns>
        public bool AddItem(Item item)
        {
            foreach (var slot in allSlots)
            {
                if (slot.IsEmpty) freeSlots--;
                if (slot.AddItem(item)) return true;
            }

            return false;
        }
        
        public int AddItems(Item item, int count)
        {
            while (count > 0)
            {
                if (!AddItem(item)) break;
                count--;
            }

            return count;
        }

        /// <summary>
        /// Removes one item from inventory
        /// </summary>
        /// <returns>Whether the provided item was successfully removed</returns>
        public bool RemoveItem(Item item)
        {
            var wasRemoved = false;
            
            foreach (var slot in allSlots)
            {
                if (slot.Item != item) continue;
                
                wasRemoved = slot.RemoveItem();

                if (slot.IsEmpty) freeSlots++;
            }

            return wasRemoved;
        }
        
        /// <summary>
        /// Removes one item with given id from inventory
        /// </summary>
        /// <returns>Whether an item with provided id was successfully removed</returns>
        public bool RemoveItem(int id)
        {
            var wasRemoved = false;
            
            foreach (var slot in allSlots)
            {
                if (slot.Item.Id != id) continue;
                
                wasRemoved = slot.RemoveItem();

                if (slot.IsEmpty) freeSlots++;
            }

            return wasRemoved;
        }

        /// <summary>
        /// Removes multiple items from inventory
        /// </summary>
        /// <returns>Count of items that were not added</returns>
        public int RemoveItems(Item item, int count)
        {
            while (count > 0)
            {
                if (!RemoveItem(item)) break;
                count--;
            }

            return count;
        }
        
        /// <summary>
        /// Removes multiple items with given id from inventory
        /// </summary>
        /// <returns>Count of items that were not added</returns>
        public int RemoveItems(int id, int count)
        {
            while (count > 0)
            {
                if (!RemoveItem(id)) break;
                count--;
            }

            return count;
        }

        public void LoadSaveData(InventorySaveData saveData)
        {
            allSlots = new List<Slot>();

            var i = 0;
            foreach (Transform child in transform)
            {
                var slotComponent = child.GetComponent<Slot>();

                var slotData = saveData.slotsSaveData[i];
                if (slotData.item != null)
                {
                    slotComponent.AddItems(slotData.item, slotData.count);
                }
                else
                {
                    freeSlots++;
                }
                
                allSlots.Add(slotComponent);
                i++;
            }
        }

        /// <summary>
        /// Only works if used on a vault inventory
        /// </summary>
        public void AddVaultSlots(int count)
        {
            if (gameObject.name != "Vault") return;

            for (int i = 0; i < count; i++)
            {
                var instance = Instantiate(_inventoryManager.slotPrefab, transform);
                
                allSlots.Add(instance.GetComponent<Slot>());
            }
            
            var backgroundRect = inventoryBackground.GetComponent<RectTransform>();
            
            var width = (cols + 3) * 7 + Metrics.SlotSize.x * cols;
            var height = (rows + 3) * 7 + Metrics.SlotSize.y * rows;
            
            backgroundRect.sizeDelta = new Vector2( width, height);

            freeSlots += count;
        }
    }
}