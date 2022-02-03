using UnityEngine;

namespace Shared
{
    /**
     * Flies forward until either the projectile hits a damage taker or leaves the game world.
     */
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        [SerializeField]
        private int damage;

        [SerializeField]
        private float despawnBoundary = 500f;

        [SerializeField, Tooltip("Tracks who fired the projectile. Optional")]
        public GameObject firer;

        private void Update()
        {
            var t = transform;
            t.position += speed * Time.deltaTime * t.forward;
            if (
                t.position.x > despawnBoundary || t.position.x < -despawnBoundary ||
                t.position.y > despawnBoundary || t.position.y < -despawnBoundary ||
                t.position.z > despawnBoundary || t.position.z < -despawnBoundary
            )
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // We don't want to hit ourselves, which can happen sometimes if the spawn location is too close to the firer
            // or the firer is moving fast.
            if (other.gameObject == firer)
            {
                return;
            }

            if (other.TryGetComponent(out IDamageTaker damageTaker))
            {
                damageTaker.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
