using System;
using Core.Converters.Effects;
using Enums;
using FullSerializer;
using UnityEngine;

namespace StatusEffects
{
	[fsObject(Processor = typeof(EffectDataProcessor),Converter = typeof(EffectDataConverter))]
	[Serializable, CreateAssetMenu(fileName = "New Effect", menuName = "Effect")]
	public class EffectData : ScriptableObject
	{
		public int id = -1;
		
		public EffectType effectType;
		public string effectName;
		public string effectDescription;
		public float effectStrength;
		public float seconds;
		public int tickDelay;
		public bool removable = true;
		public bool canStack;
	}
}