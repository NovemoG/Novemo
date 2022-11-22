using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class Stat
    {
        public Stat(string name, float baseValue)
        {
            statName = name;
            this.baseValue = baseValue;
        }
        
        [SerializeField] private string statName;
        [SerializeField] private float baseValue;

        private Dictionary<string, float> BonusValues { get; } = new();

        public float Value => baseValue + BonusValues.Sum(x => x.Value);

        public void AddBonus(string name, float value)
        {
            BonusValues[name] = baseValue * value;
        }

        public void AddFlat(float value)
        {
            baseValue += value;
        }
    }
}