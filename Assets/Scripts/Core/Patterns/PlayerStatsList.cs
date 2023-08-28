using System.Collections.Generic;
using Stats;

namespace Core.Patterns
{
	public static class PlayerStatsList
	{
		public static readonly Dictionary<string, Stat> StatsPattern = new()
		{
			{ "Health", new Stat("Health", 0) },
			{ "Mana", new Stat("Mana", 0) },
			{ "Health Regen", new Stat("Health Regen", 0) },
			{ "Mana Regen", new Stat("Mana Regen", 0) },
			{ "Physical Attack", new Stat("Physical Attack", 0) },
			{ "Ability Power", new Stat("Ability Power", 0) },
			{ "Lethal Damage", new Stat("Lethal Damage", 0) },
			{ "Attack Speed", new Stat("Attack Speed", 0) },
			{ "Crit Rate", new Stat("Crit Rate", 0) },
			{ "Crit Bonus", new Stat("Crit Bonus", 0) },
			{ "Armor", new Stat("Armor", 0) },
			{ "Magic Resist", new Stat("Magic Resist", 0) },
			{ "Movement Speed", new Stat("Movement Speed", 0) },
			{ "Cooldown Reduction", new Stat("Cooldown Reduction", 0) },
			{ "Luck", new Stat("Luck", 0) },
			{ "Exp Bonus", new Stat("Exp Bonus", 0) },
			{ "Armor Penetration", new Stat("Armor Penetration", 0) },
			{ "Magic Penetration", new Stat("Magic Penetration", 0) },
			{ "Life Steal", new Stat("Life Steal", 0) },
			{ "Ability Vampirism", new Stat("Ability Vampirism", 0) },
			{ "Counter Chance", new Stat("Counter Chance", 0) },
			{ "Double Attack Chance", new Stat("Double Attack Chance", 0) }
		};
	}
}