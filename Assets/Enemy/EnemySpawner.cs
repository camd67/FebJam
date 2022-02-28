using Player;
using Tower;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject basicEnemyPrefab;

        [SerializeField]
        private TowerHealth towerTarget;

        [SerializeField]
        private PlayerHealth playerTarget;

        [SerializeField]
        private float spawnDistance;

        [SerializeField]
        private float temporarySpawnRateSeconds;

        private void Start()
        {
            InvokeRepeating(nameof(TempSpawnEnemy), 0, temporarySpawnRateSeconds);
        }

        private void TempSpawnEnemy()
        {
            var spawnPoint2d = Random.insideUnitCircle.normalized * spawnDistance;
            var spawnPoint3d = new Vector3(spawnPoint2d.x, 0, spawnPoint2d.y);

            var spawnedEnemy = Instantiate(basicEnemyPrefab, spawnPoint3d, Quaternion.identity, transform);
            // For now, randomly switch between player and tower targets
            if (Random.value > 0.5)
            {
                spawnedEnemy.GetComponent<EnemyDirectTargetAi>().target = towerTarget.transform;
                spawnedEnemy.GetComponentInChildren<EnemyAttacker>().target = towerTarget;
            }
            else
            {
                spawnedEnemy.GetComponent<EnemyDirectTargetAi>().target = playerTarget.transform;
                spawnedEnemy.GetComponentInChildren<EnemyAttacker>().target = playerTarget;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Vector3.zero, spawnDistance);
        }
#endif
    }
}
