using System;
using FullSerializer;
using Items;
using Managers;

namespace Core.Converters.Items
{
	public class ItemDataConverter : fsDirectConverter
	{
		public override Type ModelType { get; } = typeof(ItemData);
		
		private static readonly ItemDatabase ItemDatabase = GameManager.Instance.itemDatabase;

		public override object CreateInstance(fsData data, Type storageType)
		{
			if (data.IsInt64 == false) return fsResult.Fail("Expected int in " + data);
			
			return ItemDatabase.itemsDatabase[(int)data.AsInt64];
		}

		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			serialized = new fsData(((ItemData)instance).id);
			return fsResult.Success;
		}

		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (data.IsInt64 == false) return fsResult.Fail("Expected int in " + data);

			var id = (int)data.AsInt64;
			if (id > ItemDatabase.itemsDatabase.Count - 1)
			{
				return fsResult.Fail("Id is bigger than item database's size in " + data);
			}
			
			instance = ItemDatabase.itemsDatabase[id];
			return fsResult.Success;
		}
	}
}