using System.Collections.Generic;
using Stats;

namespace Core.Patterns
{
	public static class StatsListPattern
	{
		public static readonly List<Stat> StatsPattern = new(){
			new Stat("Health", 0),
			new Stat("Mana", 0),
			new Stat("Physical Attack", 0),
			new Stat("Ability Power", 0),
			new Stat("Lethal Damage", 0),
			new Stat("Attack Speed", 0),
			new Stat("Crit Rate", 0),
			new Stat("Crit Bonus", 0),
			new Stat("Armor", 0),
			new Stat("Magic Resistance", 0),
			new Stat("Movement Speed", 0),
			new Stat("Cooldown Reduction", 0),
			new Stat("Luck", 0),
			new Stat("Exp Bonus", 0),
			new Stat("Armor Penetration", 0),
			new Stat("Magic Penetration", 0),
			new Stat("Life Steal", 0),
			new Stat("Ability Vampirism", 0),
			new Stat("Counter Chance", 0),
			new Stat("Double Attack Chance", 0)
		};
	}
}