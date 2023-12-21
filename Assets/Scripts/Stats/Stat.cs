using System;
using Enums;
using FullSerializer;
using Inventories.Stats;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class Stat
    {
        public Stat(StatName name, float baseValue)
        {
            statName = name;
            this.baseValue = baseValue;
        }
        
        [SerializeField] private StatName statName;
        /// <summary>
        /// Base value of a stat that is mostly used for bonus value calculations.
        /// </summary>
        [SerializeField] private float baseValue;
        /// <summary>
        /// Represented as a percentage of baseValue, to get full stat value us <b>Value</b>
        /// </summary>
        [SerializeField] private float bonusValue;
        
        [fsIgnore] public StatName StatName => statName;
        [fsIgnore] public float BaseValue => baseValue;
        [fsIgnore] public float BonusValue => bonusValue;
        
        [fsIgnore] public float Value => baseValue * (1 + BonusValue);

        [NonSerialized] public StatsHandler StatsList;

        /// <summary>
        /// Modifies bonus (notation: .value).
        /// </summary>
        /// <param name="value">Value by which base should be modified</param>
        public void ModifyBonus(float value)
        {
            if (value == 0) return;
            
            bonusValue += value;
            StatsList.stats[(int)statName].UpdateText(this);
        }

        /// <summary>
        /// Modifies base value of a stat by a given number
        /// </summary>
        public void ModifyFlat(float value)
        {
            if (value == 0) return;
            
            baseValue += value;
            StatsList.stats[(int)statName].UpdateText(this);
        }
    }
}