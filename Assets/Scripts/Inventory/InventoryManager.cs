using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public GameObject HoverItem;
    }
}
