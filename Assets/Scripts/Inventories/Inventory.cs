using System;
using System.Collections.Generic;
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
        
        public int freeSlots = 13;
        
        [NonSerialized]
        public List<Slot> AllSlots;

        protected virtual void Awake()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;
            AllSlots = new List<Slot>();
        }

        protected void Start()
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                AllSlots.Add(child.GetComponent<Slot>());

                if (AllSlots[i].IsEmpty) freeSlots++;

                i++;
            }

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
            foreach (var slot in AllSlots)
            {
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
            return AllSlots[slotId].RemoveItem();
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
                return true;
            }

            return false;
        }
    }
}