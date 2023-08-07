using Characters;

namespace StatusEffects
{
    public class Regen : StatusEffect
    {
        public Regen(Character character) : base(character, "HMR", -1, true)
        {
            TickCount = -1;
        }

        public override void Tick()
        {
            base.Tick();

            if (TickCount == -26)
            {
                Character.Heal(Character, Character.healthRegen, false);
                Character.RegenerateMana(Character, Character.manaRegen);
                TickCount = -1;
            }
        }
    }
}