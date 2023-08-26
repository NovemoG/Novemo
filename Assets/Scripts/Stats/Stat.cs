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
            bonusValues = new List<StatBonus>();
        }
        
        [SerializeField] private string statName;
        /// <summary>
        /// Base value of a stat that is mostly used for bonus value calculations.
        /// </summary>
        [SerializeField] private float baseValue;
        /// <summary>
        /// Container for bonus stat values for easy identification and calculation
        /// </summary>
        [SerializeField] private List<StatBonus> bonusValues;
        
        public string StatName => statName;
        public float BaseValue => baseValue;

        public float BonusValue()
        {
            if (bonusValues.Count == 0) return 0;

            return bonusValues.Sum(bonusValue => bonusValue.BonusValue);
        }
        
        public float Value => baseValue + BonusValue();
        
        public float GetBonus(string name)
        {
            if (bonusValues.Count == 0) return 0;
            
            foreach (var bonusValue in bonusValues)
            {
                if (bonusValue.BonusName == name)
                    return bonusValue.BonusValue;
            }

            return 0;
        }

        /// <summary>
        /// Adds bonus value to a BonusValues collection. Value by which base should be modified (notation: .value).
        /// </summary>
        /// <param name="name">Name of a bonus value, should be named accordingly for easy identification.</param>
        /// <param name="value">Value by which a base is modified</param>
        public void AddBonus(string name, float value)
        {
            bonusValues.Add(new StatBonus(name, baseValue * value, value));
        }

        /// <summary>
        /// Removes a bonus value(s) with a given name.
        /// </summary>
        /// <param name="name">Name of a bonus value</param>
        public void RemoveBonus(string name)
        {
            for (int i = 0; i < bonusValues.Count; i++)
            {
                if (bonusValues[i].BonusName == name)
                {
                    bonusValues.RemoveAt(i);
                    i--;
                }
            }
        }

        private void RecalculateBonuses()
        {
            foreach (var bonusValue in bonusValues)
            {
                bonusValue.Recalculate(baseValue);
            }
        }

        /// <summary>
        /// Increases base value of a stat by a given number
        /// </summary>
        public void AddFlat(float value)
        {
            baseValue += value;
            RecalculateBonuses();
        }

        /// <summary>
        /// Decreases base value of a stat by a given number
        /// </summary>
        public void RemoveFlat(float value)
        {
            baseValue -= value;
            RecalculateBonuses();
        }
    }
}