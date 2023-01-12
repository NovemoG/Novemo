using System;
using Items;

namespace Characters.Player
{
	public class Player : Character
	{
		/// <summary>
		/// Gives information about picked up item
		/// </summary>
		public event Action<Item> ItemPickedUp;
		public void InvokeItemPickedUp(Item item)
		{
			ItemPickedUp?.Invoke(item);
		}
	}
}