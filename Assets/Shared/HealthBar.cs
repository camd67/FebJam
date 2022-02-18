using System;
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

        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void LateUpdate()
        {
            healthBarSprite.fillAmount = current;

            var t = transform;
            t.rotation = cam.transform.rotation;
            t.position = followTarget.position + followOffset;
        }

        public void SetCurrent(float current)
        {
            this.current = Mathf.Clamp01(current);
        }

        public void ComputeCurrent(float current, float max)
        {
            this.current = Mathf.Clamp01(current / max);
        }
    }
}
