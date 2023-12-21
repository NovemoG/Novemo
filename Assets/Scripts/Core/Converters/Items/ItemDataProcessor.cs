using System;
using FullSerializer;
using Items;

namespace Core.Converters.Items
{
	public class ItemDataProcessor : fsObjectProcessor
	{
		public override void OnAfterSerialize(Type storageType, object instance, ref fsData data)
		{
			data = new fsData(((ItemData)instance).id);
		}
	}
}