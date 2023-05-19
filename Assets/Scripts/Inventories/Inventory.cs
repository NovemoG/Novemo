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
        protected InventoryManager InventoryManager;

        public Item one;
        public Item two;
        public Item onetwo;
        
        public int rows = 20, cols = 12;
        public int freeSlots;
        
        public Slot[,] AllSlots;
        public List<SlotGroup> UsedSlots;

        protected virtual void Awake()
        {
            InventoryManager = GameManager.Instance.InventoryManager;

            AllSlots = InventoryManager.CreateInventory(transform, rows, cols);
            UsedSlots = new List<SlotGroup>();
            freeSlots = AllSlots.Length;
            
            var inventoryRect = GetComponent<RectTransform>();
            var cellSize = GetComponent<GridLayoutGroup>().cellSize;
            inventoryRect.sizeDelta = new Vector2(cols * cellSize.x + cols - 1 + 20, rows * cellSize.y + rows - 1 + 20);
        }

        protected virtual void Update()
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
        
        public bool RemoveItem(int xSlotPosition, int ySlotPosition) => AllSlots[xSlotPosition, ySlotPosition].RemoveItem();

        public bool AddItem(Item item, int slotX = 0, int slotY = 0)
        {
            if (freeSlots == 0) return false;

            if (slotX != 0 || slotY != 0)
            {
                foreach (var slot in UsedSlots)
                {
                    //TODO doesnt add items
                    if (slot.AddItem(item)) return true;
                }
            }

            for (int i = slotX; i < rows; i++)
            {
                if (i + item.sizeRows > rows) break;
                
                for (int j = slotY; j < cols; j++)
                {
                    if (j + item.sizeCols > cols) break;

                    if (TryCreatingGroup(item, i, j)) return true;
                }
            }

            return false;
        }
        
        public List<Item> AddItems(List<Item> items, int slotX = 0, int slotY = 0)
        {
            while (items.Count > 0 && AddItem(items[0], slotX, slotY))
            {
                items.RemoveAt(0);
            }

            return items;
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
            
            var group = new SlotGroup(slots, this);
            freeSlots -= slots.Count;
            group.AddItem(item);
            UsedSlots.Add(group);
            return true;
        }
    }
}