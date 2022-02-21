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

        private float timeToRecalcDestination;
        private const float TimeBetweenRecalcs = 0.3f;

        private void Start()
        {
            navMeshAgent.destination = target.position;
        }

        private void Update()
        {
            // According to the docs, setting the destination causes a recalc of the agent path.
            // This could be expensive, so instead we'll just refresh it on an interval
            if (Time.time > timeToRecalcDestination)
            {
                navMeshAgent.destination = target.position;
                timeToRecalcDestination = Time.time + TimeBetweenRecalcs;
            }

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // Debug.Log($"{name} - Enemy is within range of target");
                // Destroy(gameObject);
            }
        }
    }
}
