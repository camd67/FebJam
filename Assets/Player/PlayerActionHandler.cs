using System;
using Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerActionHandler : MonoBehaviour
    {

        [SerializeField, Header("Camera Settings")]
        public CameraFollowStyle cameraFollowStyle = CameraFollowStyle.LockOnPlayer;

        [SerializeField]
        public float cameraFollowThreshold = 10f;

        [SerializeField]
        private GameObject cameraTarget;

        [SerializeField]
        private Camera mainCamera;

        private const float MaxCameraFollowDistanceSetting = 15f;

        [SerializeField, Header("Character Controller Settings")]
        private float moveSpeed;
        [SerializeField]
        private float lookRotationSpeed;

        [SerializeField]
        private CharacterController controller;

        [SerializeField, Header("Weapon Settings")]
        private GameObject projectilePrefab;

        [SerializeField]
        private Transform projectileSpawnLocation;

        private PlayerActionMaps playerActionMaps;

        private void Awake()
        {
            playerActionMaps ??= new PlayerActionMaps();
            playerActionMaps.Player.Enable();
            playerActionMaps.Player.Fire.performed += HandleFire;
        }

        private void OnDestroy()
        {
            playerActionMaps.Disable();
        }

        private void Update()
        {
            var transformPosition = transform.position;

            // First figure out our movement
            var moveInput = playerActionMaps.Player.Move.ReadValue<Vector2>();
            var movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
            var moveMagnitude = Mathf.Clamp01(movementDirection.magnitude) * moveSpeed;
            movementDirection.Normalize();
            // Note SimpleMove does not:
            // - allow in-air movement
            // - allow direct control of y
            // but does
            // - gravity
            controller.SimpleMove(movementDirection * moveMagnitude);

            // Calculate our rotation for the player via an invisible plane attached to the player
            var mousePos = playerActionMaps.Player.Aim.ReadValue<Vector2>();
            var playerPlane = new Plane(Vector3.up, transformPosition);
            var lookTargetRay = mainCamera.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
            if (playerPlane.Raycast(lookTargetRay, out var raycastHit))
            {
                var targetPoint = lookTargetRay.GetPoint(raycastHit);
                var targetRotation = Quaternion.LookRotation(targetPoint - transformPosition);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookRotationSpeed * Time.deltaTime);

                // Adjust the camera follow target
                if (cameraFollowStyle == CameraFollowStyle.LockOnPlayer)
                {
                    cameraTarget.transform.localPosition = Vector3.zero;
                }
                else
                {
                    var distanceToMouse = Vector3.Distance(transformPosition, targetPoint);
                    cameraTarget.transform.localPosition = new Vector3(
                        0,
                        0,
                        Mathf.Clamp(distanceToMouse, 0, cameraFollowThreshold)
                    );
                }
            }
        }

        private void HandleFire(InputAction.CallbackContext context)
        {
            Projectile.Fire(projectilePrefab, projectileSpawnLocation.position, DamageGroup.Player, gameObject);
        }

        public enum CameraFollowStyle
        {
            LockOnPlayer = 1,
            FollowCursor = 2,
        }
    }
}
