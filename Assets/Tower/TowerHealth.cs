using Shared;
using UnityEngine;

namespace Tower
{
    public class TowerHealth : MonoBehaviour, IDamageTaker
    {
        [SerializeField, Tooltip("Set automatically at startup, debug only.")]
        private float currentHealth;

        [SerializeField]
        private float initialHealth;

        private void Start()
        {
            currentHealth = initialHealth;
        }

        public void TakeDamage(float amount, DamageGroup fromGroup)
        {
            if (fromGroup == DamageGroup.Player)
            {
                return;
            }

            currentHealth -= amount;
            if (currentHealth < 0)
            {
                Destroy(gameObject);
                Debug.Log($"{name} was destroyed");
            }
        }
    }
}
