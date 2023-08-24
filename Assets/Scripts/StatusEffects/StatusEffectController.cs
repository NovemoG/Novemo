using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StatusEffects
{
	public class StatusEffectController : MonoBehaviour
	{
		public List<StatusEffect> _effects;

		private void Awake()
		{
			_effects = new List<StatusEffect>();
		}

		private void FixedUpdate()
		{
			TickEffects();
		}

		public bool ApplyEffect(StatusEffect effect)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (!effect.CanStack && _effects[i].EffectName == effect.EffectName)
				{
					return false;
				}
			}

			effect.Id = GenerateUniqueId();
			_effects.Add(effect);
			return true;
		}

		public void RemoveEffect(int id)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].Id != id) continue;
				
				_effects.RemoveAt(i);
				return;
			}
		}

		private void TickEffects()
		{
			foreach (var effect in _effects)
			{
				effect.Tick();
			}
		}

		private int GenerateUniqueId()
		{
			var excludedIds = _effects.Select(e => e.Id).ToHashSet();
			var range = Enumerable.Range(0, byte.MaxValue).Where(i => !excludedIds.Contains(i));

			var index = Random.Range(0, byte.MaxValue - excludedIds.Count);
			return range.ElementAt(index);
		}
	}
}