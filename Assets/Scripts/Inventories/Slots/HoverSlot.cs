using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories.Slots
{
    [Serializable]
    public class HoverSlot
    {
        public GameObject gameObject;
        public RectTransform rect;
        public Transform transform;
        public Slot slotClass;
        
        public GameObject background;

        public Image icon;
        public TextMeshProUGUI text;
    }
}