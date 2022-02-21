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

        [SerializeField, Tooltip("Fire rate in shots per second")]
        private float shotsPerSecond;

        [SerializeField]
        private GameObject crosshair;

        [SerializeField]
        private ParticleSystem shellParticleSystem;

        [SerializeField]
        private GameObject gunBarrel;

        private bool isFiring;

        private float barrelRotationSpeed;

        private void Awake()
        {
            playerActionMaps ??= new PlayerActionMaps();
            playerActionMaps.Player.Enable();
            // Setup our fire handlers so that the player can hold down the fire button
            // to auto-fire.
            playerActionMaps.Player.Fire.started += StartFire;
            playerActionMaps.Player.Fire.canceled += StopFire;

            // Turn the cursor off
            Cursor.visible = false;
            // but... toggle it back on if we're working on the game!
            // It's kinda annoying when the cursor is off since it's off until we unfocus the game window.
            #if UNITY_EDITOR
            Cursor.visible = true;
            #endif

            // Our gun has 6 barrels, so we need 1 rotation for every 6 shots fired (all per second)
            // Since our rotation speed is in "angles per second" we first get our angles per shot
            const int anglesPerShot = 360 / 6;
            // then figure out how many angles per second we need to go based on the number of shots per second.
            // So... if we had 6 shots per second the math would be 360 / 6 * 6 meaning we do a single rotation per second.
            barrelRotationSpeed = anglesPerShot * shotsPerSecond;
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

            var mousePos = playerActionMaps.Player.Aim.ReadValue<Vector2>();

            crosshair.transform.position = mousePos;

            // Calculate our rotation for the player via an invisible plane attached to the player
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

            if (isFiring)
            {
                gunBarrel.transform.Rotate(0, barrelRotationSpeed * Time.deltaTime, 0);
            }
        }

        private void StartFire(InputAction.CallbackContext context)
        {
            CancelInvoke(nameof(FireProjectile));
            InvokeRepeating(nameof(FireProjectile), 0, 1 / shotsPerSecond);
            isFiring = true;
        }

        private void StopFire(InputAction.CallbackContext context)
        {
            CancelInvoke(nameof(FireProjectile));
            isFiring = false;
        }

        private void FireProjectile()
        {
            Projectile.Fire(projectilePrefab, projectileSpawnLocation.position, DamageGroup.Player, gameObject);
            shellParticleSystem.Emit(1);
        }

        public enum CameraFollowStyle
        {
            LockOnPlayer = 1,
            FollowCursor = 2,
        }
    }
}
