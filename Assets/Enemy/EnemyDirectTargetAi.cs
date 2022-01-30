using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyDirectTargetAi : MonoBehaviour
    {
        [SerializeField]
        public Transform target;

        [SerializeField]
        private NavMeshAgent navMeshAgent;

        private void Start()
        {
            navMeshAgent.destination = target.position;
        }

        private void Update()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                Debug.Log($"{name} - Enemy is within range of target");
                // Destroy(gameObject);
            }
        }
    }
}
