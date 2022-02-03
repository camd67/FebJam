using System;
using Shared;
using UnityEngine;

namespace Enemy
{
    public class BasicEnemyHealth: MonoBehaviour, IDamageTaker
    {
        [SerializeField, Tooltip("Set automatically at startup, debug only.")]
        private int currentHealth;

        [SerializeField]
        private int initialHealth;

        private void Start()
        {
            currentHealth = initialHealth;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth < 0)
            {
                Destroy(gameObject);
                Debug.Log($"{name} was destroyed");
            }
        }
    }
}
