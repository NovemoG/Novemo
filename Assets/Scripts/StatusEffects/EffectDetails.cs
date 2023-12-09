using System;

namespace StatusEffects
{
	[Serializable]
	public class EffectDetails
	{
		public string effectName;
		public float seconds;
		public int tickDelay;
		public bool canStack;
		public float effectStrength;
	}
}