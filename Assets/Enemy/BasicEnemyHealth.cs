using System;
using Shared;
using UnityEngine;

namespace Enemy
{
    public class BasicEnemyHealth: MonoBehaviour, IDamageTaker
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
            if (fromGroup == DamageGroup.Enemy)
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
