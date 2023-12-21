using System;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using Enums;
using Inventories;
using Inventories.Slots;
using Inventories.Stats;
using Items;
using Loot;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static Enums.ActionCode;
using Random = UnityEngine.Random;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject slotPrefab;
        
        public Slot movingSlot;
        public GameObject movingSlotObject;

        public bool ShowTooltip { get; private set; }
        public RectTransform itemTooltipTransform;
        public TextMeshProUGUI itemTooltipText;

        public RectTransform statTooltipTransform;
        public TextMeshProUGUI statTooltipText;

        public RectTransform vaultRectTransform;
        public RectTransform chestRectTransform;
        public RectTransform equipmentRectTransform;
        
        public StatsHandler statsList;
        public Inventory playerInventory;
        public Inventory chestInventory;
        public Inventory vaultInventory;
        public EquipmentInventory equipmentInventory;

        public Transform droppedItemsParent;
        public GameObject itemDropPrefab;
        
        public ToggleGroup inventoryToggleGroup;

        [NonSerialized] public int SelectedSlotId;
        [NonSerialized] public int SelectedSlotInventory;
        
        private LootChest _currentChest;

        private Tilemap _collisionTilemap;

        public bool VaultOpen { get; private set; }
        public bool ChestOpen { get; private set; }
        public bool EquipmentOpen { get; private set; }

        public ItemData red;
        public ItemData yellow;
        public ItemData green;

        private void Start()
        {
            movingSlotObject.SetActive(false);

            _collisionTilemap = GameManager.Instance.tilemaps[1];
        }

        private void Update()
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                if (SelectedSlotId == 12)
                {
                    SelectedSlotId = 0;
                    playerInventory.allSlots[0].ToggleComponent.isOn = true;
                }
                else
                {
                    SelectedSlotId += 1;
                    playerInventory.allSlots[SelectedSlotId].ToggleComponent.isOn = true;
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                if (SelectedSlotId == 0)
                {
                    SelectedSlotId = 12;
                    playerInventory.allSlots[12].ToggleComponent.isOn = true;
                }
                else
                {
                    SelectedSlotId -= 1;
                    playerInventory.allSlots[SelectedSlotId].ToggleComponent.isOn = true;
                }
            }
            
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    playerInventory.AddItem(Create.ItemInstance(red));
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    playerInventory.AddItem(Create.ItemInstance(yellow));
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    playerInventory.AddItem(Create.ItemInstance(green));
                }

                if (Tooltip.GetKeyDown() && (!VaultOpen || !ChestOpen))
                {
                    ShowTooltip = !ShowTooltip;

                    if (!ShowTooltip)
                    {
                        itemTooltipTransform.gameObject.SetActive(false);
                    }
                }
                if (Vault.GetKeyDown())
                {
                    ToggleVault();
                }
                if (Equipment.GetKeyDown())
                {
                    ToggleEquipment();
                }
            }
        }

        public void ToggleVault()
        {
            if (VaultOpen) CloseVault();
            else OpenVault();
        }
        
        public void OpenVault()
        {
            DOTween.Kill(4);
            
            if (EquipmentOpen) CloseEquipment();
            
            VaultOpen = true;
            ShowTooltip = true;

            vaultRectTransform.gameObject.SetActive(true);
            
            DOTween.To(() => vaultRectTransform.anchoredPosition.x,
                x => vaultRectTransform.anchoredPosition = new Vector2(x, vaultRectTransform.anchoredPosition.y),
                Metrics.TargetVaultPosition.x, 0.21f).intId = 4;
        }

        public void CloseVault()
        {
            DOTween.Kill(4);
            
            VaultOpen = false;
            ShowTooltip = false;
            
            if (ChestOpen) CloseChest();

            if (SelectedSlotInventory == 1)
            {
                ClearMovingSlot();
                DeselectSlot();
            }
            
            DOTween.To(() => vaultRectTransform.anchoredPosition.x,
                x => vaultRectTransform.anchoredPosition = new Vector2(x, vaultRectTransform.anchoredPosition.y),
                Metrics.DefaultVaultPosition.x, 0.21f).OnComplete(() => vaultRectTransform.gameObject.SetActive(false)).intId = 4;
        }

        public void ToggleChest(LootChest chest)
        {
            if (ChestOpen) CloseChest();
            else OpenChest(chest);
        }

        public void OpenChest(LootChest chest)
        {
            DOTween.Kill(5);
            
            ChestOpen = true;
            chestRectTransform.gameObject.SetActive(true);
            
            _currentChest = chest;
            foreach (var loot in _currentChest.chestItems)
            {
                chestInventory.allSlots[loot.slotId].AddItems(loot.item, loot.count);
            }
            
            if (!VaultOpen) OpenVault();
            
            DOTween.To(() => chestRectTransform.anchoredPosition.x,
                x => chestRectTransform.anchoredPosition = new Vector2(x, chestRectTransform.anchoredPosition.y),
                Metrics.TargetChestPosition.x, 0.21f).intId = 5;
        }
        
        public void CloseChest()
        {
            DOTween.Kill(5);
            
            ChestOpen = false;

            _currentChest.chestItems = new List<GeneratedLoot>();
            
            for (var i = 0; i < chestInventory.allSlots.Count; i++)
            {
                var slot = chestInventory.allSlots[i];
                if (slot.IsEmpty) continue;

                _currentChest.chestItems.Add(new GeneratedLoot
                {
                    item = slot.Item,
                    count = slot.ItemCount,
                    slotId = i
                });

                slot.ClearSlot();
            }
            
            if (SelectedSlotInventory == 2)
            {
                ClearMovingSlot();
                DeselectSlot();
            }

            DOTween.To(() => chestRectTransform.anchoredPosition.x,
                x => chestRectTransform.anchoredPosition = new Vector2(x, chestRectTransform.anchoredPosition.y),
                Metrics.DefaultChestPosition.x, 0.36f).OnComplete(() => chestRectTransform.gameObject.SetActive(false)).intId = 5;
        }
        
        private void ToggleEquipment()
        {
            if (EquipmentOpen) CloseEquipment();
            else OpenEquipment();
        }

        public void CloseEquipment()
        {
            DOTween.Kill(6);

            if (SelectedSlotInventory == 3)
            {
                ClearMovingSlot();
                SelectedSlotInventory = 0;
            }
            
            EquipmentOpen = false;
            ShowTooltip = false;
            
            DOTween.To(() => equipmentRectTransform.anchoredPosition.x,
                x => equipmentRectTransform.anchoredPosition = new Vector2(x, equipmentRectTransform.anchoredPosition.y),
                Metrics.DefaultEquipmentPosition.x, 0.25f).OnComplete(() => equipmentRectTransform.gameObject.SetActive(false)).intId = 6;
        }
        
        public void OpenEquipment()
        {
            DOTween.Kill(6);
            
            if (VaultOpen) CloseVault();

            equipmentRectTransform.gameObject.SetActive(true);
            EquipmentOpen = true;
            ShowTooltip = true;
            
            DOTween.To(() => equipmentRectTransform.anchoredPosition.x,
                x => equipmentRectTransform.anchoredPosition = new Vector2(x, equipmentRectTransform.anchoredPosition.y),
                Metrics.TargetEquipmentPosition.x, 0.25f).intId = 6;
        }

        private void DeselectSlot()
        {
            SelectedSlotId = 0;
            SelectedSlotInventory = 0;
            playerInventory.allSlots[0].ToggleComponent.isOn = true;
        }

        /// <summary>
        /// Drops items randomly near given character
        /// </summary>
        /// <param name="entityPosition"></param>
        /// <param name="item"></param>
        /// <param name="count"></param>
        public void DropItems(Vector3 entityPosition, Item item, int count)
        {
            if (item == null) return;

            Vector3 dropPosition;
            
            do
            {
                var angle = Random.Range(0, Mathf.PI * 2);
                dropPosition = entityPosition + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
                
            } while (_collisionTilemap.GetTile(_collisionTilemap.WorldToCell(dropPosition)) != null);

            var dropInstance = Instantiate(itemDropPrefab, dropPosition, new Quaternion(), droppedItemsParent).GetComponent<ItemDrop>();
            dropInstance.Item = item;
            dropInstance.Count = count;
            
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
			
            var itemsLeft = from.ItemCount - to.AddItems(from.Item, from.ItemCount);
            from.RemoveItems(itemsLeft);
        }

        public void SwapItems(Slot from, Slot to)
        {
            var tempItem = to.Item;
            var tempCount = to.ItemCount;
            
            to.ClearSlot();
            to.AddItems(movingSlot.Item, movingSlot.ItemCount);
            to.SetActive();

            from.ClearSlot();
            from.AddItems(tempItem, tempCount);
        }

        public void ClearMovingSlot()
        {
            movingSlot.ClearSlot();
            movingSlotObject.SetActive(false);
        }
    }
}
