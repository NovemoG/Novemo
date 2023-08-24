using System.Collections.Generic;
using Inventories;
using Inventories.Slots;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject slotPrefab;
        
        public Slot movingSlot;
        public GameObject movingSlotObject;
        
        public Inventory playerInventory;
        public Transform playerTransform;
        public ToggleGroup inventoryToggleGroup;

        [HideInInspector] public bool keepHovering;

        private Slot _selected;

        public Item red;
        public Item yellow;
        public Item green;

        private void Start()
        {
            movingSlotObject.SetActive(false);
            
            //TODO on vault closed change selected to first inventory slot
        }

        private void Update()
        {
            if (keepHovering)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    DropItems(playerTransform, movingSlot.Items);
                }
                
                movingSlotObject.transform.position = Input.mousePosition;
            }
            
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                //select next
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                //select previous
            }
            
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    playerInventory.AddItem(red);
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    playerInventory.AddItem(yellow);
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    playerInventory.AddItem(green);
                }

                if (Input.GetKeyDown(KeyCode.V))
                {
                    
                }
            }
        }

        public void OpenVault()
        {
            
        }

        public void CloseVault()
        {
            
            //TODO if chest open firstly close chest (two of three times as fast) then close vault
            //TODO if moving slot has items from vault try to add them back, if its not possible drop them
        }
        
        /// <summary>
        /// Drops items randomly near given character
        /// </summary>
        /// <param name="characterTransform"></param>
        /// <param name="items"></param>
        public void DropItems(Transform characterTransform, List<Item> items)
        {
            if (characterTransform == null || items == null) return;
            
            movingSlot.ClearSlot();
            movingSlotObject.SetActive(false);
        }
        
        public void MergeStacks(Slot from, Slot to)
        {
            if (to.IsFull)
            {
                SwapItems(from, to);
                return;
            }
			
            to.AddItems(from.Items);
            from.UpdateSlot();
        }

        public void SwapItems(Slot from, Slot to)
        {
            var tempItems = new List<Item>(to.Items);

            to.ClearSlot();
            to.AddItems(movingSlot.Items);
            to.ToggleComponent.isOn = true;

            from.ClearSlot();
            from.AddItems(tempItems);
        }

        public void ClearMovingSlot()
        {
            movingSlot.ClearSlot();
            movingSlotObject.SetActive(false);
        }
    }
}
