using System;
using Items;

namespace Characters.Player
{
	public class Player : Character
	{
		/// <summary>
		/// Gives information about item added to inventory
		/// </summary>
		public event Action<Item> ItemCollected;
		public void InvokeItemCollected(Item item)
		{
			ItemCollected?.Invoke(item);
		}
	}
}