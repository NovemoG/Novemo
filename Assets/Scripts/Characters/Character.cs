using System;
using System.Collections.Generic;
using Core;
using Core.Patterns;
using Enums;
using Interfaces;
using Stats;
using StatusEffects;
using Unity.Collections;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(StatusEffectController))]
    public class Character : MonoBehaviour, IAttack, IDamageable
    {
        #region Stats

        private void OnValidate()
        {
            stats ??= StatsListPattern.StatsPattern;
            level = level == 0 ? 1 : level;
            EffectsController = GetComponent<StatusEffectController>();
        }

        /// <summary>
        /// Percentage values are represented by this notation: .value
        /// Stats in order (-1):
        /// <list type="number">
        /// <item><description>Health</description></item>
        /// <item><description>Mana</description></item>
        /// <item><description>Physical Attack</description></item>
        /// <item><description>Ability Power</description></item>
        /// <item><description>Lethal Damage (Percentage, applied to every type of damage, can't crit, can't apply any effects)</description></item>
        /// <item><description>Attack Speed (Attacks/second)</description></item>
        /// <item><description>Critical Rate (Percentage)</description></item>
        /// <item><description>Critical Damage Bonus (Percentage)</description></item>
        /// <item><description>Armor</description></item>
        /// <item><description>Magic Resist</description></item>
        /// <item><description>Movement Speed (Tiles/second</description></item>
        /// <item><description>Cooldown Reduction (Percentage)</description></item>
        /// <item><description>Luck (Percentage)</description></item>
        /// <item><description>Exp Bonus (Percentage)</description></item>
        /// <item><description>Armor Penetration (Percentage)</description></item>
        /// <item><description>Magic Penetration (Percentage)</description></item>
        /// <item><description>Life Steal (Percentage)</description></item>
        /// <item><description>Ability Vampirism (Percentage)</description></item>
        /// <item><description>Counter Chance (Percentage, while blocking)</description></item>
        /// <item><description>Double Attack Chance (Percentage)</description></item>
        /// </list>
        /// </summary>
        public List<Stat> stats;

        #endregion

        #region Events

        /// <summary>
        /// Invoked after character takes damage but before any calculation is made on it
        /// </summary>
        /// <para>Passes values: Source character; Damage value; Whether attack was a crit or not</para>
        public event Action<Character, DamageType, float, bool> PreMitigatedDamage;
        
        /// <summary>
        /// Invoked after character takes damage
        /// </summary>
        /// <para>Passes values: Source character; Damage value; Whether attack was a crit or not</para>
        public event Action<Character, DamageType, float, bool> DamageTaken;

        /// <summary>
        /// Invoked after character's level up
        /// </summary>
        /// <para>Passes values: Character class, current level</para>
        public event Action<Character, int> LevelUp;

        /// <summary>
        /// Triggered after character's death
        /// </summary>
        /// <para>Passes source's Character class</para>
        public event Action<Character> CharacterDeath;
        
        /// <summary>
        /// Gives information about character's health
        /// </summary>
        /// <para> Passes values (in order): Current health; Max health; Value by which health was modified</para>
        public event Action<float, float, float> HealthChange;
        public void InvokeHealthChange(float value)
        {
            CurrentHealth -= value;
            
            if (CurrentHealth < 0)
            {
                CharacterDeath?.Invoke(DamageSource);
                return;
            }
            
            HealthChange?.Invoke(MaxHealth, CurrentHealth, value);
        }
        
        /// <summary>
        /// Gives information about character's mana
        /// </summary>
        /// <para> Passes values (in order): Current mana; Max mana; Value by which mana was modified</para>
        public event Action<float, float, float> ManaChange;

        public void InvokeManaChange(float value)
        {
            CurrentMana -= value;
            ManaChange?.Invoke(MaxMana, CurrentMana, value);
        }
        
        /// <summary>
        /// Gives information about character's health
        /// </summary>
        /// <para> Passes values (in order): Current experience; Experience needed to level up; Value by which experience was modified</para>
        public event Action<int, int, int> ExperienceChange;
        public void InvokeExpChange(int value)
        {
            CurrentExp += (int)(value * (1 + stats[13].Value));
            ExperienceChange?.Invoke(ExpNeeded, CurrentExp, value);
        }

        #endregion

        private void Awake()
        {
            
        }

        private void Start()
        {
            CurrentHealth = MaxHealth;
            CurrentMana = MaxMana;
        }

        private void Update()
        {
            
        }

        public int level;

        public float MaxHealth => stats[0].Value;
        public float CurrentHealth;

        public float MaxMana => stats[1].Value;
        public float CurrentMana;
        
        public int ExpNeeded => 4 * level.Pow(3) - 8 * level * level + 25 * level - 5;
        private int _currentExp;
        public int CurrentExp
        {
            get => _currentExp;
            set
            {
                _currentExp = value;
                
                while (_currentExp > ExpNeeded)
                {
                    level += 1;
                    LevelUp?.Invoke(this, level);
                }
            }
        }
        
        public StatusEffectController EffectsController { get; private set; }

        private Character DamageSource { get; set; }
        private DateTime LastCombatAction { get; set; }

        public void DealPhysicalDamage()
        {
            
        }

        public void DealMagicDamage()
        {
            
        }
        
        /// <summary>
        /// Returns lethal damage value
        /// </summary>
        public float TakePhysicalDamage(Character source, float amount, bool isCrit)
        {
            //TODO Invoke normal attack effects (e.g. from weapon like chance for something to happen)
            if (isCrit)
            {
                amount *= 2 + source.stats[7].Value;
            }
            
            PreMitigatedDamage?.Invoke(source, DamageType.Physical, amount, isCrit);

            var lethal = amount * source.stats[4].Value;
            var armor = stats[8].Value * (1 - source.stats[14].Value);
            var damageReduction = armor/(Mathf.Pow(500 + level * 10, 0.97f) + armor);

            var final = amount * damageReduction + lethal;
            
            source.Heal(source.stats[16].Value * final);
            
            //TODO heal source by Life Steal in do damage function
            //TODO display damage
            InvokeHealthChange(final);
            DamageTaken?.Invoke(source, DamageType.Physical, final, isCrit);
            return lethal;
        }

        /// <summary>
        /// Returns lethal damage value
        /// </summary>
        public float TakeMagicDamage(Character source, float amount, bool isCrit)
        {
            if (isCrit)
            {
                amount *= 1.25f + source.stats[7].Value;
            }
            
            PreMitigatedDamage?.Invoke(source, DamageType.Magical, amount, isCrit);
            
            var lethal = amount * source.stats[4].Value;
            var mResist = stats[9].Value * (1 - source.stats[15].Value);
            var damageReduction = mResist/(Mathf.Pow(600 + level * 10, 0.98f) + mResist);

            var final = amount * damageReduction + lethal;
            
            source.Heal(source.stats[17].Value * final);
            
            InvokeHealthChange(final);
            DamageTaken?.Invoke(source, DamageType.Magical, final, isCrit);
            return lethal;
        }

        public void Heal(float amount)
        {
            
        }
    }
}
