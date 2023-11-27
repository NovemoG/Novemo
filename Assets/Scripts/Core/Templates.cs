using Enums;

namespace Core
{
	public static class Templates
	{
		public static string FormatItemName(string itemName, Rarity itemRarity)
		{
			switch (itemRarity)
			{
				case Rarity.Common:
					return $"<color=#DFFF00>{itemName}</color>";
				case Rarity.Rare:
					return $"<color=#5D3FD3>{itemName}</color>";
				case Rarity.Epic:
					return $"<color=#89CFF0><i>{itemName}</i></color>";
				case Rarity.Legendary:
					return $"<color=#E69E19><size=30><i>{itemName}</i></size></color>";
				case Rarity.Mythic:
					return $"<color=#FFFFFF><size=32><b><i>{itemName}</i></b></size></color>";
				case Rarity.None:
				default:
					return $"<color=#A6A6A6>{itemName}</color>";
			}
		}
		
		/// <summary>
		/// 0 - Item Name<br/>
		/// 1 - Item Description<br/>
		/// 2 - Item Count<br/>
		/// 3 - Item Stack Limit<br/>
		/// 4 - Item Type<br/>
		/// </summary>
		public const string ItemTooltipTemplate = "{0}\n{1}\n<size=20>Stack limit: {2}</size>\n<color=#28282888><size=14><i>{3}</i></size></color>";
	}
}