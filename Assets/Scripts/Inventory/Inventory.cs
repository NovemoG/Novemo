using System.Collections.Generic;
using Core;
using Inventory.Slots;
using Items;
using Managers;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        private InventoryManager _inventoryManager;

        public Item one;
        public Item two;
        public Item onetwo;
        
        public int rows = 20, cols = 12;
        public int freeSlots;
        
        public Slot[,] AllSlots;
        public List<SlotGroup> UsedSlots;

        protected virtual void Awake()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;

            AllSlots = new Slot[rows,cols];
            UsedSlots = new List<SlotGroup>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var slot = Instantiate(_inventoryManager.slotObject, transform, true);
                    var slotClass = slot.GetComponent<Slot>();
                    
                    slot.gameObject.name = $"Slot";
                    slotClass.slotCoordinates = new Coordinates2D(i, j);

                    AllSlots[i, j] = slotClass;
                    freeSlots++;
                }
            }
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                AddItem(one);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                AddItem(two);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                AddItem(onetwo);
            }
        }

        public bool AddItem(Item item)
        {
            if (freeSlots == 0) return false;
            
            for (int i = 0; i < UsedSlots.Count; i++)
            {
                var slot = UsedSlots[i].PivotSlot;

                if (slot.AddItem(item)) return true;
            }

            for (int i = 0; i < rows; i += item.sizeRows)
            {
                if (i + item.sizeRows > rows) break;
                
                for (int j = 0; j < cols; j += item.sizeCols)
                {
                    if (j + item.sizeCols > cols) break;
                    
                    Debug.Log($"{i}, {j}");

                    if (TryCreatingGroup(item, i, j)) return true;
                }
            }

            return false;
        }

        public bool TryCreatingGroup(Item item, int startX, int startY)
        {
            var slots = new List<Slot>();
            
            for (int i = 0; i < item.sizeRows; i++)
            {
                for (int j = 0; j < item.sizeCols; j++)
                {
                    var current = AllSlots[startX + i, startY + j];

                    if (current.IsInGroup) return false;
                    slots.Add(current);
                }
            }
            
            var group = new SlotGroup(slots);
            freeSlots -= slots.Count;
            group.AddItem(item);
            UsedSlots.Add(group);
            return true;
        }
    }
}