using System;
using System.Collections.Generic;
using System.Linq;
using Inventories.Slots;
using Items;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventories
{
    public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler/*, IInitializePotentialDragHandler*/
    {
        private InventoryManager _inventoryManager;
        private Transform _playerTransform;
        private GameObject _movingObject;
        private Slot _startingSlot;
        private Slot _movingSlot;

        private void Awake()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;
            _playerTransform = GameManager.Instance.PlayerManager.playerObject.transform;
            _movingObject = _inventoryManager.movingSlotObject;
            _movingSlot = _inventoryManager.movingSlot;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            foreach (var current in eventData.hovered)
            {
                if (current.name != "Slot") continue;

                _startingSlot = current.GetComponent<Slot>();
                
                if (_startingSlot.IsEmpty)
                {
                    _startingSlot = null;
                    return;
                }
                
                _movingSlot.AddItems(_startingSlot.Items);
                _movingObject.SetActive(true);
                return;
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _movingObject.transform.position = Input.mousePosition;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            var dropItems = true;
            
            foreach (var current in eventData.hovered)
            {
                if (_startingSlot == null) return;
                if (current.name != "Slot") continue;
                if (current == _startingSlot.gameObject)
                {
                    dropItems = false;
                    break;
                }

                var currentSlot = current.GetComponent<Slot>();
                
                if (currentSlot.Peek != _startingSlot.Peek)
                {
                    var tempItems = new List<Item>(currentSlot.Items);
                    
                    currentSlot.ClearSlot();
                    currentSlot.AddItems(_movingSlot.Items);
                    currentSlot.ToggleComponent.isOn = true;
                    
                    _startingSlot.ClearSlot();
                    _startingSlot.AddItems(tempItems);

                    dropItems = false;
                    break;
                }

                //If hovered slot does have items from moving slot add them
                var rest = currentSlot.AddItems(_movingSlot.Items);

                if (rest.Count == 0) break;
                    
                //else keep hovering with what left
                _inventoryManager.keepHovering = true;

                _startingSlot = null;
                _movingSlot.RemoveItems(_movingSlot.Items.Count - rest.Count);
                return;
            }
            
            if (eventData.hovered.Count == 0 && !_movingSlot.IsEmpty && dropItems)
            {
                _inventoryManager.DropItems(_playerTransform, _movingSlot.Items);
                _startingSlot.ClearSlot();
            }
            
            _startingSlot = null;
            _movingSlot.ClearSlot();
            _movingObject.SetActive(false);
        }

        /*public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }*/
    }
}