using System;
using Inventories.Slots;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler
    {
        private GameObject _movingObject;
        private Slot _movingSlot;
        private Slot _selected;

        private void Awake()
        {
            var inventoryManager = GameManager.Instance.InventoryManager;
            _movingObject = inventoryManager.movingSlot;
            _movingSlot = inventoryManager.movingSlot.GetComponent<Slot>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("test");
            
            foreach (var current in eventData.hovered)
            {
                if (current.name != "Slot") continue;

                _selected = current.GetComponent<Slot>();
                
                Debug.Log("slot selected");

                for (int i = 0; i < _selected.Count; i++)
                {
                    var wasAdded = _movingSlot.AddItem(_selected.Peek);
                    
                    Debug.Log($"{wasAdded} adding items");
                    
                    if (!wasAdded) return;
                    
                    _movingObject.SetActive(true);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            foreach (var current in eventData.hovered)
            {
                if (current.name != "Slot") continue;
                if (current == _selected.gameObject) break;

                
            }
            
            //remove items from moving slot and deselect
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
    }
}