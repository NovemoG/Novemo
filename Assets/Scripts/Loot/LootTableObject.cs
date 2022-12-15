using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
	[CreateAssetMenu(fileName = "Loot")]
	public class LootTableObject : ScriptableObject
	{
		public List<Loot> loot;
	}
}