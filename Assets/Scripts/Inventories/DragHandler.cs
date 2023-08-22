using Inventories.Slots;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
    public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler/*, IInitializePotentialDragHandler*/
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
            foreach (var current in eventData.hovered)
            {
                if (current.name != "Slot") continue;

                _selected = current.GetComponent<Slot>();

                for (int i = 0; i < _selected.ItemCount; i++)
                {
                    var wasAdded = _movingSlot.AddItem(_selected.Peek);
                    
                    if (!wasAdded) return;
                    
                    _movingObject.SetActive(true);
                }
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _movingObject.transform.position = Input.mousePosition;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            foreach (var current in eventData.hovered)
            {
                if (current.name != "Slot") continue;
                if (current == _selected.gameObject) break;

                var selected = current.GetComponent<Slot>();
                
                for (int i = 0; i < _movingSlot.ItemCount; i++)
                {
                    var wasAdded = selected.AddItem(_movingSlot.Peek);
                    
                    Debug.Log($"{wasAdded} adding static items");
                    
                    if (!wasAdded) break;
                    
                    _movingObject.SetActive(true);
                }
            }

            //drop items
            
            _selected = null;
            
            _movingSlot.ClearSlot();
            _movingObject.SetActive(false);
        }

        /*public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }*/
    }
}