using UnityEngine;

namespace Shared
{
    public interface IDamageTaker
    {
        /**
         * Takes damage with the given amount and from which group.
         */
        void TakeDamage(float amount, DamageGroup fromGroup);

        Vector3 getLocation();
    }
}
