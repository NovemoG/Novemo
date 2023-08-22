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
        public GameObject movingSlot;
        
        public Inventory playerInventory;
        public ToggleGroup inventoryToggleGroup;

        private Slot _selected;

        public Item red;
        public Item yellow;
        public Item green;

        private void Start()
        {
            movingSlot.SetActive(false);
            
            //TODO on vault closed change selected to first inventory slot
        }

        private void Update()
        {
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
            
            
            //TODO if moving slot has items from vault try to add them back, if its not possible drop them
        }

        public void DropItems(Transform characterTransform, List<Item> items)
        {
            
        }
    }
}
