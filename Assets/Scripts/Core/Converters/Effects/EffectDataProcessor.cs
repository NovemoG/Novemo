using System;
using FullSerializer;
using StatusEffects;

namespace Core.Converters.Effects
{
	public class EffectDataProcessor : fsObjectProcessor
	{
		public override void OnAfterSerialize(Type storageType, object instance, ref fsData data)
		{
			data = new fsData(((EffectData)instance).id);
		}
	}
}