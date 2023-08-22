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
        
        [NonSerialized]
        public List<Slot> AllSlots;

        protected virtual void Awake()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;
            AllSlots = new List<Slot>();

            for (int i = 0; i < slotCount; i++)
            {
                var instance = Instantiate(_inventoryManager.slotPrefab, transform);

                AddSlot(instance);
            }
        }

        protected void Start()
        {
            //TODO load items from save file
            
            //TODO after save is loaded change '0' to last selected slot
            AllSlots[0].GetComponent<Toggle>().isOn = true;
        }

        protected virtual void Update()
        {
            
        }

        /// <summary>
        /// Tries to add one item to inventory
        /// </summary>
        /// <returns>A boolean value that represents whether the operation was successful</returns>
        public bool AddItem(Item item)
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
        /// Tries to add one item to a given slot id (Note: it will try to add provided item to other slot if possible)
        /// </summary>
        /// <returns>A boolean value that represents whether the operation was successful</returns>
        public bool AddItem(Item item, int slotId)
        {
            return AllSlots[slotId].AddItem(item) || AddItem(item);
        }
        
        /// <summary>
        /// Removes one item from a given slot id
        /// </summary>
        /// <returns>A boolean value that represents whether the operation was successful</returns>
        public bool RemoveItem(int slotId)
        {
            var wasRemoved = AllSlots[slotId].RemoveItem();

            if (AllSlots[slotId].IsEmpty) freeSlots++;
            
            return wasRemoved;
        }

        /// <summary>
        /// Only use it if u want to check for quest items or items that need to be removed to complete some task
        /// </summary>
        /// <returns>A boolean value that represents whether the provided item was successfully removed</returns>
        public bool RemoveItem(Item item)
        {
            foreach (var slot in AllSlots)
            {
                if (slot.Peek != item) continue;
                
                slot.RemoveItem();

                if (slot.IsEmpty) freeSlots++;
                
                return true;
            }

            return false;
        }

        public void AddSlot(GameObject slotInstance)
        {
            AllSlots.Add(slotInstance.GetComponent<Slot>());
            
            var backgroundRect = inventoryBackground.GetComponent<RectTransform>();
            
            var height = (rows + 3) * 7 + Metrics.SlotSize.y * rows;
            var width = (cols + 3) * 7 + Metrics.SlotSize.x * cols;
            
            backgroundRect.sizeDelta = new Vector2( width, height);
            
            freeSlots++;
        }
    }
}