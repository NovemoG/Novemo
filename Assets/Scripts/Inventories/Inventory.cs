using System;
using System.Collections.Generic;
using Core;
using Inventories.Slots;
using Items;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories
{
    public class Inventory : MonoBehaviour
    {
        private InventoryManager _inventoryManager;

        public GameObject inventoryBackground;
        public int rows, cols, slotCount;
        
        public int freeSlots;
        
        [NonSerialized] public List<Slot> AllSlots;

        protected virtual void Awake()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;
            AllSlots = new List<Slot>();

            foreach (Transform child in transform)
            {
                AllSlots.Add(child.GetComponent<Slot>());

                freeSlots++;
            }
        }

        protected void Start()
        {
            //TODO load items from save file
            
            //TODO after save is loaded change '0' to last selected slot (in inventory)
            if (gameObject.name == "Inventory")
            {
                AllSlots[0].GetComponent<Toggle>().isOn = true;
            }
        }

        protected virtual void Update()
        {
            
        }

        /// <summary>
        /// Tries to add one item to inventory
        /// </summary>
        /// <returns>A boolean value that represents whether the provided item was successfully added</returns>
        public virtual bool AddItem(Item item)
        {
            if (freeSlots == 0) return false;
            
            foreach (var slot in AllSlots)
            {
                if (slot.IsEmpty) freeSlots--;
                if (slot.AddItem(item)) return true;
            }

            return false;
        }

        /// <summary>
        /// Adds multiple items to inventory from provided list while simultaneously removing items one by one from provided list 
        /// </summary>
        /// <returns>List of items that left which couldn't be added</returns>
        public virtual int AddItems(Item item, int count)
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
        /// <returns>A boolean value that represents whether the provided item was successfully removed</returns>
        public virtual bool RemoveItem(Item item)
        {
            var wasRemoved = false;
            
            foreach (var slot in AllSlots)
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
        /// <returns>A boolean value that represents whether the provided item was successfully removed</returns>
        public virtual bool RemoveItem(int referenceId)
        {
            var wasRemoved = false;
            
            foreach (var slot in AllSlots)
            {
                if (slot.Item.ReferenceId != referenceId) continue;
                
                wasRemoved = slot.RemoveItem();

                if (slot.IsEmpty) freeSlots++;
            }

            return wasRemoved;
        }

        /// <summary>
        /// Removes multiple items from inventory
        /// </summary>
        /// <returns>A boolean value that represents whether function removed items or it is not possible to do so</returns>
        public virtual int RemoveItems(Item item, int count)
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
        /// <returns>A boolean value that represents whether function removed items or it is not possible to do so</returns>
        public virtual int RemoveItems(int referenceId, int count)
        {
            while (count > 0)
            {
                if (!RemoveItem(referenceId)) break;
                count--;
            }

            return count;
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
                
                AllSlots.Add(instance.GetComponent<Slot>());
            }
            
            var backgroundRect = inventoryBackground.GetComponent<RectTransform>();
            
            var width = (cols + 3) * 7 + Metrics.SlotSize.x * cols;
            var height = (rows + 3) * 7 + Metrics.SlotSize.y * rows;
            
            backgroundRect.sizeDelta = new Vector2( width, height);

            freeSlots += count;
        }
    }
}