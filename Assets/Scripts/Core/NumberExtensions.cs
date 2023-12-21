using UnityEngine;

namespace Core
{
	public static class NumberExtensions
	{
		/// <summary>
		/// Compares float values
		/// </summary>
		/// <param name="val1">First value</param>
		/// <param name="val2">Second Value</param>
		/// <returns></returns>
		public static bool CompareWith(this float val1, float val2)
		{
			return Mathf.Abs(val1 - val2) < 0.001f;
		}

		public static int Pow(this int x, short power)
		{
			return (int)Mathf.Pow(x, power);
		}
	}
}