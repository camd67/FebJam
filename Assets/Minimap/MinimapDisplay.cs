using UnityEngine;
using UnityEngine.UI;

namespace Minimap
{
    public class MinimapDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image enemyImage;

        [SerializeField]
        private GameObject enemyMinimapParent;

        [SerializeField]
        private Image minimapBackground;

        [SerializeField]
        private Transform enemyOwner;

        [SerializeField]
        private float maxTrackedDistance;

        [SerializeField]
        private Transform player;

        [SerializeField]
        private Image playerImage;

        private readonly Image[] enemiesOnMapPool = new Image[NumEnemiesInPool];

        private const int NumEnemiesInPool = 50;
        private readonly Vector2 offscreenPos = new(-100, -100);

        private float maxRenderDistance;

        private void Start()
        {
            for (var enemyIdx = 0; enemyIdx < NumEnemiesInPool; enemyIdx++)
            {
                enemiesOnMapPool[enemyIdx] = Instantiate(enemyImage, Random.insideUnitCircle * 10, Quaternion.identity, enemyMinimapParent.transform);
            }

            maxRenderDistance = minimapBackground.rectTransform.rect.width / 2;
        }

        private Vector2? ComputeMinimapCoord(Transform t)
        {
            var position = t.position;
            // get distance from center
            var distance = position.magnitude;
            if (distance > maxTrackedDistance)
            {
                return null;
            }
            // get direction from center
            var dir = position.normalized;
            // figure out how far we are from our max render distance
            var distanceRatio = distance / maxTrackedDistance;
            // calculate that distance from the center
            return new Vector2(dir.x, dir.z) * maxRenderDistance * distanceRatio;
        }

        void Update()
        {
            var playerLocation = ComputeMinimapCoord(player);
            playerImage.rectTransform.anchoredPosition = playerLocation ?? offscreenPos;

            var enemiesRendered = 0;
            for (var i = 0; i < enemyOwner.childCount && enemiesRendered < NumEnemiesInPool; i++)
            {
                var enemyTransform = enemyOwner.GetChild(i);

                var imageLocation = ComputeMinimapCoord(enemyTransform);
                if (!imageLocation.HasValue)
                {
                    continue;
                }

                var pooledEnemyImage = enemiesOnMapPool[enemiesRendered];
                pooledEnemyImage.rectTransform.anchoredPosition = imageLocation.Value;
                enemiesRendered++;
            }

            for (var i = enemiesRendered; i < NumEnemiesInPool; i++)
            {
                enemiesOnMapPool[i].rectTransform.anchoredPosition = offscreenPos;
            }
        }
    }
}
