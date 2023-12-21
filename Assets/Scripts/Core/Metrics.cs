using System;
using System.IO;
using UnityEngine;

namespace Core
{
    public static class Metrics
    {
        public static readonly string GameDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Novemo");
        public static readonly string SavesPath = Path.Combine(GameDataPath, "Saves");
        
        public static Vector2 SlotSize = new(75, 75);
        
        public static Vector2 DefaultVaultPosition = new(297.5f, -445);
        public static Vector2 TargetVaultPosition = new(-297.5f, -445);
        
        public static Vector2 DefaultChestPosition = new(215.5f, -445);
        public static Vector2 TargetChestPosition = new(-803.5f, -445);

        public static Vector2 DefaultEquipmentPosition = new(500, -432);
        public static Vector2 TargetEquipmentPosition = new(-493, -432);

        public static int ExpNeededFormula(int level) => 4 * level.Pow(3) - 8 * level.Pow(2) + 25 * level;
        
        public const int CurrentTweenId = 6;
        
        public const int ChestSize = 40;
    }
}