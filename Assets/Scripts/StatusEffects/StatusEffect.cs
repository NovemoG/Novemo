using System;
using Characters;
using FullSerializer;
using UnityEngine;

namespace StatusEffects
{
	[Serializable]
	public class StatusEffect
	{
		public StatusEffect(EffectData effectData)
		{
			this.effectData = effectData;
			
			if (effectData.seconds < 0)
			{
				tickCount = effectTickLength = -1;
			}
			else
			{
				tickCount = effectTickLength = (int)(effectData.seconds * 50);
			}

			effectDescription = effectData.effectDescription;
		}

		public bool paused;
		[SerializeField] protected EffectData effectData;
		[SerializeField] protected int id;
		[SerializeField] protected int tickCount;
		[SerializeField] protected int effectTickLength;
		[SerializeField] protected string effectDescription;

		[fsIgnore] public Character Character { get; set; }
		
		[fsIgnore] public EffectData EffectData => effectData;
		[fsIgnore] public int Id => id;
		[fsIgnore] public int TickDelay => effectData.tickDelay;
		[fsIgnore] public bool CanStack => effectData.canStack;
		[fsIgnore] public string EffectName => effectData.effectName;
		[fsIgnore] public float EffectStrength => effectData.effectStrength;
		[fsIgnore] public virtual string FormattedDescription => $"{effectDescription}\n{GetEffectLength}{IsStackable}";

		[fsIgnore] protected string GetEffectLength => effectTickLength < 0
				? "Length: <b>Infinite</b>"
				: $"Length: {effectTickLength / 50}s";

		[fsIgnore] protected string IsStackable => CanStack ? string.Empty : "\n<i><size=16>This effect doesn't stack</size><i>";

		public void SetId(int newId)
		{
			if (id != 0) return;

			id = newId;
		}

		public void Reset()
		{
			Character = null;
			id = 0;
			
			if (effectData.seconds < 0)
			{
				tickCount =  -1;
			}
			else
			{
				tickCount = (int)(effectData.seconds * 50);
			}
		}

		public void Tick()
		{
			if (tickCount == 0)
			{
				EndEffect();
			}
			
			tickCount -= 1;
			
			if (tickCount == TickDelay) OnTick();
		}

		public virtual void OnTick()
		{
			tickCount = effectTickLength == -1 ? -1 : 0;
		}
		
		public virtual void EndEffect()
		{
			tickCount = 0;
			
			Character.RemoveEffect(id);
		}
	}
}