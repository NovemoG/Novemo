using Characters;
using UnityEngine;

namespace StatusEffects
{
	public class StatusEffect : ScriptableObject
	{
		//TODO fix this
		[HideInInspector] public int id;
		[HideInInspector] public bool paused;
		[HideInInspector] public Character character;
		
		[SerializeField] protected string effectDescription;
		[SerializeField] protected EffectDetails effectDetails;
		
		protected int TickCount;
		protected int TickDelay;
		protected int EffectLength;
		protected float EffectStrength;
		public bool CanStack { get; private set; }
		public string EffectName { get; private set; }
		public virtual string FormattedDescription
		{
			get
			{
				var value = string.Format(effectDescription);

				return value + GetEffectLength;
			}
		}

		protected string GetEffectLength => effectDetails.seconds < 0
				? "<color=#282828><size=20>Length: <b>Infinite</b></size></color>"
				: $"<color=#282828><size=20>Length: {effectDetails.seconds}s</size></color>";

		protected virtual void OnEnable()
		{
			if (effectDetails.seconds < 0)
			{
				TickCount = EffectLength = -1;
			}
			else
			{
				TickCount = EffectLength = (int)(effectDetails.seconds * 50);
			}
			
			TickDelay = effectDetails.tickDelay;
			CanStack = effectDetails.canStack;
			EffectName = effectDetails.effectName;
			EffectStrength = effectDetails.effectStrength;
		}

		public void Tick()
		{
			if (paused) return;
			if (TickCount == 0)
			{
				EndEffect();
				
				character.EffectsController.RemoveEffect(id);
			}
			
			TickCount -= 1;

			if (TickCount == TickDelay) OnTick();
		}

		public virtual void OnTick() { }
		
		public virtual void EndEffect()
		{
			TickCount = 0;
		}
	}
}