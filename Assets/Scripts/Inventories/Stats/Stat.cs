using TMPro;
using UnityEngine;

namespace Inventories.Stats
{
	[RequireComponent(typeof(StatTooltipHandler))]
	public class Stat : MonoBehaviour
	{
		public string statName;
		public string details;
		public int statIndex;
		public TextMeshProUGUI statText;
		
		public void UpdateText(float baseValue, float bonusValue, float value)
		{
			statText.text = details == "f+%"
				? $"{statName}: {baseValue} + {bonusValue}%"
				: $"{statName}: {value}{details}";
		}
	}
}