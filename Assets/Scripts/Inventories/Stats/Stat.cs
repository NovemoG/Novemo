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
		
		public void UpdateText(global::Stats.Stat stat)
		{
			switch (details)
			{
				case "f+%":
					statText.text = $"{statName}: {stat.BaseValue}";
					if (stat.BonusValue > 0) statText.text += $" + {stat.BonusValue * 100}%";
					break;
				case "%":
					statText.text = $"{statName}: {stat.Value * 100}%";
					break;
				default:
					statText.text = $"{statName}: {stat.Value}{details}";
					break;
			}
		}
	}
}