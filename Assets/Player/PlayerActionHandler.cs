using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerActionHandler : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float lookRotationSpeed;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private CharacterController controller;

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
            var playerPlane = new Plane(Vector3.up, transform.position);
            var lookTargetRay = mainCamera.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
            if (playerPlane.Raycast(lookTargetRay, out var raycastHit))
            {
                var targetPoint = lookTargetRay.GetPoint(raycastHit);
                var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                // we only want top-down rotation so lock the other axes
                targetPoint.x = 0;
                targetPoint.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookRotationSpeed * Time.deltaTime);
            }
        }

        private void HandleFire(InputAction.CallbackContext context)
        {
            Debug.Log($"Fire - {context.phase}");
        }
    }
}
