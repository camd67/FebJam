using Shared;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeField]
        private float attackDamage;

        [SerializeField]
        private DamageGroup damageGroup = DamageGroup.Enemy;

        [SerializeField]
        private float attacksPerSecond;

        [SerializeField]
        private AudioClip[] attackClips;

        [SerializeField]
        private AudioSource attackAudioSource;

        private float lastAttackTime;

        public IDamageTaker target;

        private void OnTriggerStay(Collider other)
        {
            var attackTime = 1 / attacksPerSecond;
            if (
                Time.time > lastAttackTime + attackTime
                && other.gameObject.TryGetComponent(out IDamageTaker damageTaker)
                && damageTaker == target
                )
            {
                lastAttackTime = Time.time;
                target.TakeDamage(attackDamage, damageGroup);
                attackAudioSource.clip = attackClips[Random.Range(0, attackClips.Length)];
                attackAudioSource.pitch = Random.Range(0.5f, 1.5f);
                attackAudioSource.Play();
            }
        }
    }
}
