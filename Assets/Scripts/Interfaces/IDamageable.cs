using Characters;
using Enums;

namespace Interfaces
{
    public interface IDamageable
    {
        public float TakeDamage(Character source, DamageType type, float amount, bool isCrit)
        {
            return 0;
        }
    }
}