using UnityEngine;

namespace Core
{
    public static class Metrics
    {
        public static readonly Vector2 SlotSize = new(30, 30);

        public static void SetSlotSize(RectTransform imageRect, RectTransform backgroundRect, int itemSizeX, int itemSizeY)
        {
            if (itemSizeX > 0)
            {
                imageRect.offsetMin = new Vector2(2, 1 - SlotSize.x * itemSizeX);
                backgroundRect.offsetMin = new Vector2(0, -(1 + SlotSize.x) * itemSizeX);
            }

            if (itemSizeY > 0)
            {
                imageRect.offsetMax = new Vector2(-(1 - SlotSize.y * itemSizeY), -2);
                backgroundRect.offsetMax = new Vector2((1 + SlotSize.x) * itemSizeY, 0);
            }
        }
    }
}