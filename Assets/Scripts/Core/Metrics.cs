using UnityEngine;

namespace Core
{
    public static class Metrics
    {
        public static Vector2 SlotSize = new(75, 75);
        
        public static Vector2 DefaultVaultPosition = new(297.5f, -445);
        public static Vector2 TargetVaultPosition = new(-297.5f, -445);
        
        public static Vector2 DefaultChestPosition = new(215.5f, -445);
        public static Vector2 TargetChestPosition = new(-803.5f, -445);
        
        public const int ChestSize = 40;
    }
}