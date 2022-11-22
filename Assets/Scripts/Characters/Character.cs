using System.Collections.Generic;
using Interfaces;
using Stats;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour, IDamageable, IAttack
    {
        public static Character Instance;
        
        private void Awake()
        {
            Instance = this;
        }

        public int level;

        /// <summary>
        /// <para>0 - Health</para>
        /// <para>1 - Mana</para>
        /// <para>2 - Physical Attack</para>
        /// <para>3 - Ability Power</para>
        /// <para>4 - Lethal Damage (Percentage)</para>
        /// <para>5 - Attack Speed</para>
        /// <para>6 - Critical Rate</para>
        /// <para>7 - Critical Damage Bonus (Percentage)</para>
        /// <para>8 - Armor</para>
        /// <para>9 - Magic Resist</para>
        /// <para>10 - Movement Speed (Tiles/second)</para>
        /// <para>11 - Cooldown Reduction</para>
        /// <para>12 - Luck</para>
        /// <para>13 - Exp Bonus</para>
        /// <para>14 - Armor Penetration (Percentage)</para>
        /// <para>15 - Magic Penetration (Percentage)</para>
        /// <para>16 - Life Steal</para>
        /// <para>17 - Ability Vampirism</para>
        /// <para>18 - Block Chance (Only blocks attack)</para>
        /// <para>19 - Pair Chance (Also counters)</para>
        /// </summary>
        public List<Stat> stats;

        public void TakeDamage(Character source, int value)
        {
            //Calculate Pair/Block

            var lethal = value * source.stats[4].Value;
            var armor = stats[8].Value * (1 - source.stats[14].Value);
            
            var damageReduction = armor/(Mathf.Pow(500 + level * 10, 0.97f) + armor);
            
            var final = value * damageReduction + lethal;

            //Calculate for crit and apply multiplier
            //Apply damage
            //Display damage
        }
    }
}
