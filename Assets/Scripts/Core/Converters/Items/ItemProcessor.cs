using System;
using FullSerializer;
using Items;

namespace Core.Converters.Items
{
	public class ItemProcessor : fsObjectProcessor
	{
		public override void OnAfterDeserialize(Type storageType, object instance)
		{
			((Item)instance)?.GenerateTooltip();
		}
	}
}