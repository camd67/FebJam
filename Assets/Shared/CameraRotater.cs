using UnityEngine;

namespace Shared
{
    public class CameraRotater : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        void Update()
        {
            transform.Rotate(0, Time.deltaTime * speed, 0);
        }
    }
}
