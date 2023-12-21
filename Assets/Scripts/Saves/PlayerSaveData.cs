using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Stats;
using StatusEffects;

namespace Saves
{
	[Serializable]
	public class PlayerSaveData
	{
		public PlayerSaveData(Player player)
		{
			level = player.Level;
			currentExp = player.CurrentExp;

			currentHealth = player.CurrentHealth;
			currentMana = player.CurrentMana;

			healthRegenEffect = player.HealthRegenEffect;
			manaRegenEffect = player.ManaRegenEffect;

			stats = player.Stats;
			effects = player.EffectsController.Effects.ToList();
			pausedEffects = player.EffectsController.PausedEffects.ToList();
		}
		
		public int level;
		public int currentExp;

		public float currentHealth;
		public float currentMana;

		public EffectData healthRegenEffect;
		public EffectData manaRegenEffect;
		
		public List<Stat> stats;
		public List<StatusEffect> effects;
		public List<StatusEffect> pausedEffects;
	}
}