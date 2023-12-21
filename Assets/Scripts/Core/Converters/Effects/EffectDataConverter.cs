using System;
using FullSerializer;
using Managers;
using StatusEffects;

namespace Core.Converters.Effects
{
	public class EffectDataConverter : fsDirectConverter
	{
		public override Type ModelType { get; } = typeof(EffectData);

		private static readonly ItemDatabase ItemDatabase = GameManager.Instance.itemDatabase;

		public override object CreateInstance(fsData data, Type storageType)
		{
			if (data.IsInt64 == false) return fsResult.Fail("Expected int in " + data);
			
			return ItemDatabase.effectsDatabase[(int)data.AsInt64];
		}

		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			serialized = new fsData(((EffectData)instance).id);
			return fsResult.Success;
		}

		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (data.IsInt64 == false) return fsResult.Fail("Expected int in " + data);
			
			var id = (int)data.AsInt64;
			if (id > ItemDatabase.effectsDatabase.Count - 1)
			{
				return fsResult.Fail("Id is bigger than effect database's size in " + data);
			}
			
			instance = ItemDatabase.effectsDatabase[id];
			return fsResult.Success;
		}
	}
}