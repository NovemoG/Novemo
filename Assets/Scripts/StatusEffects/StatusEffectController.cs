using System.Collections.Generic;
using UnityEngine;

namespace StatusEffects
{
	public class StatusEffectController : MonoBehaviour
	{
		private List<StatusEffect> _effects;

		private void Awake()
		{
			_effects = new List<StatusEffect>();
		}

		private void FixedUpdate()
		{
			EveryTick();
		}

		public bool ApplyEffect(StatusEffect effect)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].EffectName == effect.EffectName && !effect.CanStack)
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
				if (_effects[i].Id == id)
				{
					_effects.RemoveAt(i);
					return;
				}
			}
		}

		private void EveryTick()
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				_effects[i].Tick();
			}
		}

		private int GenerateUniqueId()
		{
			var id = Random.Range(0, short.MaxValue);
			
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].Id != id) continue;
				
				id = Random.Range(0, short.MaxValue);
				i = 0;
			}

			return id;
		}
	}
}