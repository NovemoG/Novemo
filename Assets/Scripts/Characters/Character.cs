using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Patterns;
using Enums;
using Interfaces;
using Stats;
using StatusEffects;
using UI;
using Unity.Mathematics;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(StatusEffectController))]
    [RequireComponent(typeof(DisplayDamage))]
    public class Character : MonoBehaviour, IDamageable
    {
        #region Stats

        private void OnValidate()
        {
            stats = StatsListPattern.StatsPattern;
            EffectsController = GetComponent<StatusEffectController>();
        }

        /// <summary>
        /// Percentage values are represented by this notation: .value<br/>
        /// Stats in order:<br/>
        /// 0 - Health<br/>
        /// 1 - Mana<br/>
        /// 2 - Physical Attack<br/>
        /// 3 - Ability Power<br/>
        /// 4 - Lethal Damage (Percentage, applied to every type of damage, can't crit, can't apply any effects)<br/>
        /// 5 - Attack Speed (Attacks/second)<br/>
        /// 6 - Critical Rate (Percentage)<br/>
        /// 7 - Critical Damage Bonus (Percentage)<br/>
        /// 8 - Armor<br/>
        /// 9 - Magic Resist<br/>
        /// 10 - Movement Speed (Tiles/second)<br/>
        /// 11 - Cooldown Reduction (Percentage)<br/>
        /// 12 - Luck (Percentage)<br/>
        /// 13 - Exp Bonus (Percentage)<br/>
        /// 14 - Armor Penetration (Percentage)<br/>
        /// 15 - Magic Penetration (Percentage)<br/>
        /// 16 - Life Steal (Percentage)<br/>
        /// 17 - Ability Vampirism (Percentage)<br/>
        /// 18 - Counter Chance (Percentage, while blocking)<br/>
        /// 19 - Double Attack Chance (Percentage)
        /// </summary>
        [SerializeField]
        public List<Stat> stats;

        public float MaxHealth => stats[0].Value;
        public float CurrentHealth { get; private set; }
        public float healthRegen;

        public float MaxMana => stats[1].Value;
        public float CurrentMana { get; private set; }
        public float manaRegen;

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
        /// <summary>
        /// Function used to change character's health value. To decrease health use -value
        /// </summary>
        public void InvokeHealthChange(float value)
        {
            CurrentHealth += value;
            
            if (CurrentHealth < 0)
            {
                CharacterDeath?.Invoke(DamageSource);
                CurrentHealth = 0;
            }
            
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            
            HealthChange?.Invoke(CurrentHealth, MaxHealth, value);
        }
        
        /// <summary>
        /// Gives information about character's mana
        /// </summary>
        /// <para> Passes values (in order): Current mana; Max mana; Value by which mana was modified</para>
        public event Action<float, float, float> ManaChange;
        /// <summary>
        /// Function used to change character's mana value. To decrease mana use -value
        /// </summary>
        public void InvokeManaChange(float value)
        {
            CurrentMana += value;

            if (CurrentMana > MaxMana)
            {
                CurrentMana = MaxMana;
            }
            
            ManaChange?.Invoke(CurrentMana, MaxMana, value);
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

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            InvokeHealthChange(MaxHealth);
            InvokeManaChange(MaxMana);
            level = level > 1 ? level : 1;
            LevelUp?.Invoke(this, level);
            
            if (healthRegen != 0 || manaRegen != 0)
            {
                var regenEffect = new Regen(this);
                    
                EffectsController.ApplyEffect(regenEffect);
            }
        }

        protected virtual void Update()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    TakeDamage(this, DamageType.Physical, 5, false);
                    /*InvokeHealthChange(-2);
                    DamageTaken?.Invoke(this, DamageType.Physical, 2, false);*/
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    Heal(this, 2, true);
                }
            }
        }

        public int level;

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

        /// <summary>
        /// Returns damage taken
        /// </summary>
        public float TakeDamage(Character source, DamageType type, float amount, bool isCrit)
        {
            if (isCrit)
            {
                amount *= 1.5f + source.stats[7].Value;
            }

            PreMitigatedDamage?.Invoke(source, type, amount, isCrit);
            
            var lethal = amount * source.stats[4].Value;
            var defense = type == DamageType.Physical
                ? stats[8].Value * (1 - source.stats[14].Value)
                : stats[9].Value * (1 - source.stats[15].Value);
            var damageReduction = defense / math.pow(500 + defense, 0.97f);
            
            Debug.Log(1 - damageReduction);
            
            var final = amount * (1 - damageReduction) + lethal;
            
            Debug.Log(final);
            
            //TODO life steal/spell vampirism source heals on attack
            //source.Heal(source, source.stats[16].Value * final, false);
            
            InvokeHealthChange(-final);
            DamageTaken?.Invoke(source, type, final, isCrit);
            return final;
        }

        public void Heal(Character source, float amount, bool isCrit)
        {
            if (isCrit)
            {
                amount *= 1.5f;
            }
            
            InvokeHealthChange(amount);
            DamageTaken?.Invoke(source, DamageType.Heal, amount, isCrit);
        }

        public void RegenerateMana(Character source, float amount)
        {
            InvokeManaChange(amount);
            DamageTaken?.Invoke(source, DamageType.Mana, amount, false);
        }
    }
}
