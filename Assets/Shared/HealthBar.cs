using UnityEngine;
using UnityEngine.UI;

namespace Shared
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Transform followTarget;

        [SerializeField]
        private Vector3 followOffset;

        [SerializeField, Tooltip("Current health value. Range of 0-1")]
        private float current;

        [SerializeField]
        private Image healthBarSprite;

        [SerializeField]
        private Image healthBarBackgroundSprite;

        [SerializeField, Tooltip("Hides the healthbar if at 100%")]
        private bool hideAtFull = true;

        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
            if (hideAtFull && Mathf.Approximately(current, 1))
            {
                healthBarSprite.enabled = false;
                healthBarBackgroundSprite.enabled = false;
            }
        }

        private void Update()
        {
            healthBarSprite.fillAmount = current;
        }

        private void LateUpdate()
        {

            var t = transform;
            t.rotation = cam.transform.rotation;
            t.position = followTarget.position + followOffset;
        }

        public void SetCurrent(float current)
        {
            this.current = Mathf.Clamp01(current);
            if (hideAtFull && Mathf.Approximately(this.current, 1))
            {
                healthBarSprite.enabled = false;
                healthBarBackgroundSprite.enabled = false;
            }
            else
            {
                healthBarSprite.enabled = true;
                healthBarBackgroundSprite.enabled = true;
            }
        }

        public void ComputeCurrent(float current, float max)
        {
            SetCurrent(current / max);
        }
    }
}
