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

        [SerializeField]
        private HealthBar healthBar;

        private void Start()
        {
            currentHealth = initialHealth;
            healthBar.ComputeCurrent(currentHealth, initialHealth);
        }

        public void TakeDamage(float amount, DamageGroup fromGroup)
        {
            if (fromGroup == DamageGroup.Player)
            {
                return;
            }

            currentHealth -= amount;
            healthBar.ComputeCurrent(currentHealth, initialHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"{name} was destroyed");
            }
        }

        public Vector3 getLocation()
        {
            return transform.position;
        }
    }
}
