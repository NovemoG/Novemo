using System.Collections.Generic;
using System.Linq;
using Core;
using Inventories;
using Inventories.Slots;
using Items;
using Loot;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject slotPrefab;
        
        public Slot movingSlot;
        public GameObject movingSlotObject;

        public RectTransform vaultRectTransform;
        public RectTransform chestRectTransform;
        public Inventory chestInventory;
        
        public Inventory playerInventory;
        public Transform playerTransform;
        public ToggleGroup inventoryToggleGroup;

        [HideInInspector] public bool keepHovering;

        private bool _vaultOpen;
        private bool _chestOpen;

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
                    ToggleVault();
                }
            }
        }

        public void ToggleVault()
        {
            if (_vaultOpen) CloseVault();
            else OpenVault();
        }
        
        private void OpenVault()
        {
            LeanTween.cancel(vaultRectTransform);
            
            _vaultOpen = true;
            
            LeanTween.moveX(vaultRectTransform, Metrics.TargetVaultPosition.x, 0.33f);
        }

        private void CloseVault()
        {
            LeanTween.cancel(vaultRectTransform);
            
            _vaultOpen = false;
            
            if (_chestOpen) CloseChest();
            
            LeanTween.moveX(vaultRectTransform, Metrics.DefaultVaultPosition.x, 0.21f);
            
            //TODO if moving slot has items from vault try to add them back, if its not possible drop them
        }

        public void ToggleChest(LootChest chest)
        {
            if (_chestOpen) CloseChest();
            else OpenChest(chest);
        }

        private void OpenChest(LootChest chest)
        {
            LeanTween.cancel(chestRectTransform);
            
            _chestOpen = true;
            
            foreach (var loot in chest.lootTable.GeneratedLoot)
            {
                chestInventory.AllSlots[loot.SlotId].AddItems(loot.Item, loot.Count);
            }
            
            if (!_vaultOpen) OpenVault();
            
            LeanTween.moveX(chestRectTransform, Metrics.TargetChestPosition.x, 0.33f);
        }
        
        private void CloseChest()
        {
            LeanTween.cancel(chestRectTransform);
            
            _chestOpen = false;

            foreach (var slot in chestInventory.AllSlots)
            {
                slot.Items.Clear();
            }

            LeanTween.moveX(chestRectTransform, Metrics.DefaultChestPosition.x, 0.36f).setOnComplete(() =>
            {
                foreach (var slot in chestInventory.AllSlots.Where(slot => !slot.IsEmpty))
                {
                    slot.ClearSlot();
                }
            });
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
