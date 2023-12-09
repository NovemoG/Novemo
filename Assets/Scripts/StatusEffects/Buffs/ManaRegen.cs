using UnityEngine;

namespace StatusEffects.Buffs
{
    [CreateAssetMenu(fileName = "New Mana Regen Effect", menuName = "Effects/Mana Regen")]
    public class ManaRegen : StatusEffect
    {
        public override void OnTick()
        {
            character.RegenerateMana(character, EffectStrength == 0 ? character.ManaRegen : EffectStrength);
            TickCount = EffectLength == -1 ? -1 : TickCount;
        }
    }
}