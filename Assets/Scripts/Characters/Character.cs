using System;
using System.Collections.Generic;
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
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Character : MonoBehaviour, IDamageable
    {
        #region Stats

        private void OnValidate()
        {
            if (stats == null) stats = StatsListPattern.StatsPattern;
        }
        
        /// <summary>
        /// Percentage values are represented by this notation: 0.value<br/>
        /// Stats in order:<br/>
        /// 0 - Health<br/>
        /// 1 - Mana<br/>
        /// 2 - Health Regen<br/>
        /// 3 - Mana Regen<br/>
        /// 4 - Physical Attack<br/>
        /// 5 - Ability Power<br/>
        /// 6 - Lethal Damage (Percentage, applied to every type of damage, can't crit, can't apply any effects)<br/>
        /// 7 - Attack Speed (Attacks/second)<br/>
        /// 8 - Crit Rate (Percentage)<br/>
        /// 9 - Crit Bonus (Percentage)<br/>
        /// 10 - Armor<br/>
        /// 11 - Magic Resist<br/>
        /// 12 - Movement Speed (Tiles/second)<br/>
        /// 13 - Cooldown Reduction (Percentage)<br/>
        /// 14 - Luck (Percentage)<br/>
        /// 15 - Exp Bonus (Percentage)<br/>
        /// 16 - Armor Penetration<br/>
        /// 17 - Magic Penetration<br/>
        /// 18 - Life Steal<br/>
        /// 19 - Ability Vampirism<br/>
        /// 20 - Counter Chance (Percentage, while blocking)<br/>
        /// 21 - Double Attack Chance (Percentage)
        /// </summary>
        [ListElementTitle("statName")] [SerializeField] protected List<Stat> stats;
        /// <summary>
        /// Percentage values are represented by this notation: 0.value<br/>
        /// Stats in order:<br/>
        /// 0 - Health<br/>
        /// 1 - Mana<br/>
        /// 2 - Health Regen<br/>
        /// 3 - Mana Regen<br/>
        /// 4 - Physical Attack<br/>
        /// 5 - Ability Power<br/>
        /// 6 - Lethal Damage (Percentage, applied to every type of damage, can't crit, can't apply any effects)<br/>
        /// 7 - Attack Speed (Attacks/second)<br/>
        /// 8 - Crit Rate (Percentage)<br/>
        /// 9 - Crit Bonus (Percentage)<br/>
        /// 10 - Armor<br/>
        /// 11 - Magic Resist<br/>
        /// 12 - Movement Speed (Tiles/second)<br/>
        /// 13 - Cooldown Reduction (Percentage)<br/>
        /// 14 - Luck (Percentage)<br/>
        /// 15 - Exp Bonus (Percentage)<br/>
        /// 16 - Armor Penetration (Percentage)<br/>
        /// 17 - Magic Penetration (Percentage)<br/>
        /// 18 - Life Steal (Percentage)<br/>
        /// 19 - Ability Vampirism (Percentage)<br/>
        /// 20 - Counter Chance (Percentage, while blocking)<br/>
        /// 21 - Double Attack Chance (Percentage)
        /// </summary>
        public List<Stat> Stats => stats;
        
        public float MaxHealth => stats[0].Value;
        public float CurrentHealth { get; private set; }
        public float HealthRegen => stats[2].Value;

        public float MaxMana => stats[1].Value;
        public float CurrentMana { get; private set; }
        public float ManaRegen => stats[3].Value;

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
        protected void ModifyCharacterHealth(float value)
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
        /// Called when character's max health changes
        /// </summary>
        public void UpdateHealthData()
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            
            HealthChange?.Invoke(CurrentHealth, MaxHealth, 0);
        }
        
        /// <summary>
        /// Gives information about character's mana
        /// </summary>
        /// <para> Passes values (in order): Current mana; Max mana; Value by which mana was modified</para>
        public event Action<float, float, float> ManaChange;
        
        /// <summary>
        /// Function used to change character's mana value. To decrease mana use -value
        /// </summary>
        protected void ModifyCharacterMana(float value)
        {
            CurrentMana += value;

            if (CurrentMana > MaxMana)
            {
                CurrentMana = MaxMana;
            }
            
            ManaChange?.Invoke(CurrentMana, MaxMana, value);
        }
        
        /// <summary>
        /// Called when character's max mana changes
        /// </summary>
        public void UpdateManaData()
        {
            CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
            
            ManaChange?.Invoke(CurrentMana, MaxMana, 0);
        }
        
        /// <summary>
        /// Gives information about character's health
        /// </summary>
        /// <para> Passes values (in order): Current experience; Experience needed to level up; Value by which experience was modified</para>
        public event Action<int, int, int> ExperienceChange;
        public void InvokeExpChange(int value)
        {
            CurrentExp += (int)(value * (1 + stats[14].Value));
            ExperienceChange?.Invoke(ExpNeeded, CurrentExp, value);
        }

        #endregion
        
        [SerializeField] protected int level;
        public int Level => level;
        
        [SerializeField] protected int currentExp;
        public int CurrentExp
        {
            get => currentExp;
            private set
            {
                currentExp = value;
                
                while (currentExp > ExpNeeded)
                {
                    level += 1;
                    ExpNeeded = Metrics.ExpNeededFormula(level);
                    LevelUp?.Invoke(this, level);
                }
            }
        }
        public int ExpNeeded { get; private set; }

        [SerializeField] protected EffectData healthRegenEffect;
        public EffectData HealthRegenEffect => healthRegenEffect;
        
        [SerializeField] protected EffectData manaRegenEffect;
        public EffectData ManaRegenEffect => manaRegenEffect;

        protected Rigidbody2D Rigidbody2D;
        protected SpriteRenderer SpriteRenderer;
        public StatusEffectController EffectsController { get; private set; }

        public Character DamageSource { get; set; }
        public DateTime LastCombatAction { get; set; }
        
        protected virtual void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            EffectsController = GetComponent<StatusEffectController>();
            
            ExpNeeded = Metrics.ExpNeededFormula(level);
        }

        protected virtual void Start()
        {
            if (!EffectsController.ContainsEffect(healthRegenEffect))
            {
                EffectsController.ApplyEffect(healthRegenEffect).Character = this;
            }

            if (!EffectsController.ContainsEffect(manaRegenEffect))
            {
                EffectsController.ApplyEffect(manaRegenEffect).Character = this;
            }
        }

        protected virtual void Update()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    TakeDamage(this, AttackType.Normal_Attack, DamageType.Physical, 5, false);
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    Heal(this, 2, true);
                }
            }
        }

        /// <summary>
        /// Returns damage taken
        /// </summary>
        public float TakeDamage(Character source, AttackType aType, DamageType dType, float amount, bool isCrit)
        {
            if (isCrit)
            {
                amount *= 1.5f + source.stats[9].Value;
            }

            PreMitigatedDamage?.Invoke(source, dType, amount, isCrit);
            
            var lethal = amount * source.stats[6].Value;
            var defense = dType == DamageType.Physical
                ? stats[10].Value * (1 - source.stats[16].Value)
                : stats[11].Value * (1 - source.stats[17].Value);
            var damageReduction = defense / math.pow(500 + defense, 0.97f);
            
            var final = amount * (1 - damageReduction) + lethal;
            
            if (aType == AttackType.Normal_Attack)
            {
                source.Heal(source, source.stats[18].Value * final, false);
            }
            else if (aType == AttackType.Ability) //DoTs don't heal
            {
                source.Heal(source, source.stats[19].Value * final, false);
            }
            
            DamageSource = source;
            ModifyCharacterHealth(-final);
            DamageTaken?.Invoke(source, dType, final, isCrit);
            return final;
        }

        public void Heal(Character source, float amount, bool isCrit)
        {
            if (isCrit)
            {
                amount *= 1.5f;
            }
            
            ModifyCharacterHealth(amount);
            DamageTaken?.Invoke(source, DamageType.Heal, amount, isCrit);
        }

        public void RegenerateMana(Character source, float amount)
        {
            ModifyCharacterMana(amount);
            DamageTaken?.Invoke(source, DamageType.Mana, amount, false);
        }

        public void ApplyEffect(StatusEffect effect)
        {
            effect.Character = this;
            EffectsController.ApplyEffect(effect);
        }

        public void RemoveEffect(int id)
        {
            EffectsController.RemoveEffect(id);
        }
    }
}
