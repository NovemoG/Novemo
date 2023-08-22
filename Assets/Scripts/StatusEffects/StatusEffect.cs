using Characters;

namespace StatusEffects
{
	public class StatusEffect
	{
		public int Id;
		
		public readonly string EffectName;
		public readonly bool CanStack;

		protected readonly Character Character;
		protected int TickCount;

		protected StatusEffect(Character character, string effectName, float seconds, bool canStack)
		{
			Character = character;
			EffectName = effectName;
			TickCount = (int)(seconds * 50);
			CanStack = canStack;
		}

		public virtual void Tick()
		{
			if (TickCount == 0)
			{
				EffectEnd();
				
				Character.EffectsController.RemoveEffect(Id);
			}
			
			TickCount -= 1;
		}

		public virtual void EffectEnd()
		{
			
		}
	}
}