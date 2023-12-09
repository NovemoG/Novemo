using System.Collections.Generic;
using System.Linq;
using Core;
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

			effect.id = UniqueId.Generate(_effects.Select(e => e.id).ToHashSet(), 0, byte.MaxValue);
			_effects.Add(Instantiate(effect));
			return true;
		}

		/// <summary>
		/// Pauses an effect with given id
		/// </summary>
		/// <param name="id">Id of applied effect</param>
		/// <returns>Whether effect was paused or not</returns>
		public bool PauseEffect(int id)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].id != id) continue;
				
				_effects[i].paused = true;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Unpauses an effect with given id
		/// </summary>
		/// <param name="id">Id of applied effect</param>
		/// <returns>Whether effect was unpaused or not</returns>
		public bool UnpauseEffect(int id)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].id != id) continue;
				
				_effects[i].paused = false;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Finishes an effect with given id and fires EndEffect method
		/// </summary>
		/// <param name="id">Id of applied effect</param>
		/// <returns>Whether effect was finished or not</returns>
		public bool FinishEffect(int id)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].id != id) continue;
				
				_effects[i].EndEffect();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Removes an effect without firing EndEffect method
		/// </summary>
		/// <param name="id">Id of applied effect</param>
		/// <returns>Whether effect was removed or not</returns>
		public bool RemoveEffect(int id)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].id != id) continue;
				
				_effects.RemoveAt(i);
				return true;
			}

			return false; 
		}

		private void TickEffects()
		{
			for (var i = 0; i < _effects.Count; i++)
			{
				_effects[i].Tick();
			}
		}
	}
}