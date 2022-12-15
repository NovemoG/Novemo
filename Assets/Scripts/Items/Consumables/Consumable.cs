using Interfaces;
using UnityEngine;

namespace Items.Consumables
{
	[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Consumables/Consumable")]
	public class Consumable : Item, IUsable
	{
		
	}
}