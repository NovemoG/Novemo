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
            _bonusValues = new Dictionary<string, float>();
        }
        
        [SerializeField] private string statName;
        /// <summary>
        /// Base value of a stat that is mostly used for bonus value calculations.
        /// </summary>
        [SerializeField] private float baseValue;
        /// <summary>
        /// Container for bonus stat values for easy identification and calculation
        /// </summary>
        private Dictionary<string, float> _bonusValues;
        
        public string StatName => statName;
        public float BaseValue => baseValue;
        public float GetBonus(string name) => _bonusValues[name];
        public float Value => baseValue + _bonusValues?.Sum(x => x.Value) ?? baseValue;

        /// <summary>
        /// Adds bonus value to a BonusValues collection. Value by which base should be modified (notation: .value)
        /// </summary>
        /// <param name="name">Name of a bonus value, should be named accordingly for easy identification.</param>
        /// <param name="value">Value by which a base is modified</param>
        public void AddBonus(string name, float value) => _bonusValues[name] = baseValue * value;

        /// <summary>
        /// Removes a bonus value with a given name.
        /// </summary>
        /// <param name="name">Name of a bonus value</param>
        public void RemoveBonus(string name) => _bonusValues.Remove(name);

        /// <summary>
        /// Increases base value of a stat by a given number
        /// </summary>
        public void AddFlat(float value) => baseValue += value;

        /// <summary>
        /// Decreases base value of a stat by a given number
        /// </summary>
        public void RemoveFlat(float value) => baseValue -= value;
    }
}