using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class StatBonus
    {
        public readonly string BonusName;

        [SerializeField] private float bonusValue;
        public float BonusValue => bonusValue;

        private readonly float _percentage;

        public StatBonus(string bonusName, float bonusValue, float percentage)
        {
            BonusName = bonusName;
            this.bonusValue = bonusValue;
            _percentage = percentage;
        }

        public void Recalculate(float baseValue)
        {
            bonusValue = baseValue * _percentage;
        }
    }
}