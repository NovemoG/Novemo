using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Characters;
using Core;
using Saves;
using UnityEngine;

namespace StatusEffects
{
	public class StatusEffectController : MonoBehaviour
	{
		private List<StatusEffect> _effects;
		public ReadOnlyCollection<StatusEffect> Effects => _effects.AsReadOnly();
		
		private List<StatusEffect> _pausedEffects;
		public ReadOnlyCollection<StatusEffect> PausedEffects => _pausedEffects.AsReadOnly();

		private void Awake()
		{
			_effects ??= new List<StatusEffect>();
			_pausedEffects ??= new List<StatusEffect>();
		}

		private void FixedUpdate()
		{
			TickEffects();
		}

		public StatusEffect ApplyEffect(EffectData effect)
		{
			var id = UniqueId.Generate(_effects.Select(e => e.Id).ToHashSet(), 1, short.MaxValue);
			var effectInstance = Create.EffectInstance(effect);

			effectInstance.SetId(id);
			
			for (int i = 0; i < _effects.Count; i++)
			{
				if (!effect.canStack && _effects[i].EffectData.id == effect.id)
				{
					effectInstance.paused = true;
					_pausedEffects.Add(effectInstance);
					return effectInstance;
				}
			}
			
			_effects.Add(effectInstance);
			return effectInstance;
		}

		public void ApplyEffect(StatusEffect effect)
		{
			var id = UniqueId.Generate(_effects.Select(e => e.Id).ToHashSet(), 1, byte.MaxValue);
			
			effect.SetId(id);
			
			for (int i = 0; i < _effects.Count; i++)
			{
				if (!effect.CanStack && _effects[i].EffectData.id == effect.EffectData.id)
				{
					effect.paused = true;
					_pausedEffects.Add(effect);
					return;
				}
			}
			
			_effects.Add(effect);
		}

		public bool ContainsEffect(EffectData effectData)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].EffectData.id == effectData.id)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Pauses first effect with given id that is not yet paused
		/// </summary>
		/// <param name="id">Id of applied effect</param>
		public void PauseEffect(int id)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].Id != id) continue;

				_effects[i].paused = true;
				_pausedEffects.Add(_effects[i]);
				_effects.RemoveAt(i);
				return;
			}
		}

		/// <summary>
		/// Unpauses first effect with given id that is currently paused
		/// </summary>
		/// <param name="id">Id of applied effect</param>
		public void UnpauseEffect(int id)
		{
			for (int i = 0; i < _pausedEffects.Count; i++)
			{
				if (_pausedEffects[i].Id != id & _pausedEffects[i].paused) continue;

				_pausedEffects[i].paused = false;
				_effects.Add(_pausedEffects[i]);
				_pausedEffects.RemoveAt(i);
				return;
			}
		}

		/// <summary>
		/// Unpauses first effect with given effect data that is currently paused
		/// </summary>
		/// <param name="effect">Effect data used to find other paused effects of the same type</param>
		public void UnpauseEffect(EffectData effect)
		{
			for (int i = 0; i < _pausedEffects.Count; i++)
			{
				if (_pausedEffects[i].EffectData.id != effect.id & _pausedEffects[i].paused) continue;
				
				_pausedEffects[i].paused = false;
				_effects.Add(_pausedEffects[i]);
				_pausedEffects.RemoveAt(i);
				return;
			}
		}

		/// <summary>
		/// Finishes first effect with given id and fires EndEffect method
		/// </summary>
		/// <param name="id">Id of applied effect</param>
		public void FinishEffect(int id)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].Id != id) continue;
				if (!_effects[i].EffectData.removable) return;
				
				_effects[i].EndEffect();
				return;
			}
		}

		/// <summary>
		/// Removes first effect without firing EndEffect method
		/// </summary>
		/// <param name="id">Id of applied effect</param>
		public void RemoveEffect(int id)
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (_effects[i].Id != id) continue;
				if (!_effects[i].EffectData.removable) return;
				
				UnpauseEffect(_effects[i].EffectData);
				_effects[i].Reset();
				_effects.RemoveAt(i);
				return;
			}
		}

		public void LoadSaveData(PlayerSaveData saveData)
		{
			_effects = saveData.effects;
			_pausedEffects = saveData.pausedEffects;

			var character = GetComponent<Character>();
			for (int i = 0; i < _effects.Count; i++)
			{
				_effects[i].Character = character;
			}

			for (int i = 0; i < _pausedEffects.Count; i++)
			{
				_pausedEffects[i].Character = character;
			}
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