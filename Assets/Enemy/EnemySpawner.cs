using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject basicEnemyPrefab;

        [SerializeField]
        private Transform towerTarget;

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

            var spawnedEnemy = Instantiate(basicEnemyPrefab, spawnPoint3d, Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyDirectTargetAi>().target = towerTarget;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(towerTarget.position, spawnDistance);
        }
#endif
    }
}
