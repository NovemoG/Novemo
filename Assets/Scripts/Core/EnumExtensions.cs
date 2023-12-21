using System;

namespace Core
{
	public static class EnumExtensions
	{
		public static T ToEnum<T>(this string enumValue)
		{
			return (T)Enum.Parse(typeof(T), enumValue);
		}

		public static T ToEnum<T>(this string enumValue, T defaultValue) where T : struct
		{
			if (string.IsNullOrEmpty(enumValue))
			{
				return defaultValue;
			}

			return Enum.TryParse(enumValue, true, out T result) ? result : defaultValue;
		}
	}
}