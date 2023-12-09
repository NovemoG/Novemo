using UnityEngine;

namespace StatusEffects.Buffs
{
	[CreateAssetMenu(fileName = "New Health Regen Effect", menuName = "Effects/Health Regen")]
	public class HealthRegen : StatusEffect
	{
		public override void OnTick()
		{
			character.Heal(character, EffectStrength == 0 ? character.HealthRegen : EffectStrength, false);
			TickCount = EffectLength == -1 ? -1 : TickCount;
		}
	}
}