using UnityEngine;

namespace Player
{
    public class PlayerCameraFollower : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private float cameraHeight;

        void Update()
        {
            var pos = transform.position;
            mainCamera.transform.position = new Vector3(pos.x, cameraHeight, pos.z);
        }
    }
}
