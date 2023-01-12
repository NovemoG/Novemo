using System;
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
		/// <param name="diff">Maximum given difference between two compared float values</param>
		/// <returns></returns>
		public static bool CompareWith(this float val1, float val2, float diff)
		{
			return Mathf.Abs(val1 - val2) < diff;
		}

		public static int Pow(this int x, short power)
		{
			return (int)Math.Pow(x, power);
		}
	}
}