using System;
using Shared;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IDamageTaker
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
                Debug.Log("Player was destroyed");
            }
        }
    }
}
