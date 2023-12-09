using System.Collections.Generic;
using Enums;
using Stats;

namespace Core.Patterns
{
	public static class StatsListPattern
	{
		public static readonly List<Stat> StatsPattern = new()
		{
			new Stat(StatName.Health, 0),
			new Stat(StatName.Mana, 0),
			new Stat(StatName.Health_Regen, 0),
			new Stat(StatName.Mana_Regen, 0),
			new Stat(StatName.Physical_Attack, 0),
			new Stat(StatName.Ability_Power, 0),
			new Stat(StatName.Lethal_Damage, 0),
			new Stat(StatName.Attack_Speed, 0),
			new Stat(StatName.Crit_Rate, 0),
			new Stat(StatName.Crit_Bonus, 0),
			new Stat(StatName.Armor, 0),
			new Stat(StatName.Magic_Resist, 0),
			new Stat(StatName.Movement_Speed, 0),
			new Stat(StatName.Cooldown_Reduction, 0),
			new Stat(StatName.Luck, 0),
			new Stat(StatName.Exp_Bonus, 0),
			new Stat(StatName.Armor_Penetration, 0),
			new Stat(StatName.Magic_Resist_Penetration, 0),
			new Stat(StatName.Life_Steal, 0),
			new Stat(StatName.Ability_Vampirism, 0),
			new Stat(StatName.Counter_Chance, 0),
			new Stat(StatName.Double_Attack_Chance, 0)
		};
	}
}