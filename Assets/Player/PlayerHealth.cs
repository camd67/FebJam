using System;
using Shared;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IDamageTaker
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
                Debug.Log("Player was destroyed");
            }
        }
    }
}
