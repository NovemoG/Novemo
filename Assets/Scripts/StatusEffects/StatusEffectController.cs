using System.Collections.Generic;
using UnityEngine;

namespace StatusEffects
{
	public class StatusEffectController : MonoBehaviour
	{
		public List<StatusEffect> Effects;

		private void Awake()
		{
			Effects = new List<StatusEffect>();
		}

		private void FixedUpdate()
		{
			EveryTick();
		}

		public bool ApplyEffect(StatusEffect effect)
		{
			for (int i = 0; i < Effects.Count; i++)
			{
				if (Effects[i].EffectName == effect.EffectName && !effect.CanStack)
				{
					return false;
				}
			}

			effect.Id = GenerateUniqueId();
			Effects.Add(effect);
			return true;
		}

		public void RemoveEffect(int id)
		{
			for (int i = 0; i < Effects.Count; i++)
			{
				if (Effects[i].Id == id)
				{
					Effects.RemoveAt(i);
					return;
				}
			}
		}

		private void EveryTick()
		{
			for (int i = 0; i < Effects.Count; i++)
			{
				Effects[i].Tick();
			}
		}

		private int GenerateUniqueId()
		{
			var id = Random.Range(0, short.MaxValue);
			
			for (int i = 0; i < Effects.Count; i++)
			{
				if (Effects[i].Id != id) continue;
				
				id = Random.Range(0, short.MaxValue);
				i = 0;
			}

			return id;
		}
	}
}