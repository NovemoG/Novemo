using UnityEngine;

namespace StatusEffects.Buffs
{
    public class ManaRegen : StatusEffect
    {
        public ManaRegen(EffectData effectData) : base(effectData)
        {
            effectDescription = string.Format(effectDescription, EffectStrength, Mathf.Abs((float)effectData.tickDelay / 50));
            effectDescription += effectData.tickDelay is 5 or 50 ? " second" : " seconds";

            if (effectTickLength > 0)
            {
                effectDescription += $" for {effectData.seconds} seconds";
            }
        }
        
        public override void OnTick()
        {
            base.OnTick();
            
            Character.RegenerateMana(Character, EffectStrength == 0 ? Character.ManaRegen : EffectStrength);
        }
    }
}