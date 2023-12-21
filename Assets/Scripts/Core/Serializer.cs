using System;
using FullSerializer;

namespace Core
{
	public static class Serializer
	{
		private static readonly fsSerializer _serializer = new();

		public static string Serialize(Type type, object value)
		{
			_serializer.TrySerialize(type, value, out var data).AssertSuccessWithoutWarnings();
			
			return fsJsonPrinter.PrettyJson(data);
		}

		public static object Deserialize(Type type, string serializedState)
		{
			var data = fsJsonParser.Parse(serializedState);
			
			object deserialized = null;
			_serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

			return deserialized;
		}
	}
}