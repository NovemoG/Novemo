using Loot;

namespace Inventory
{
	public class LootChest : Inventory
	{
		public LootStruct loot;

		protected override void Awake()
		{
			base.Awake();
			
			//TODO fill with loot in random slots
		}
	}
}