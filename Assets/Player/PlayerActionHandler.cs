using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerActionHandler : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        private PlayerActionMaps playerActionMaps;

        private void Awake()
        {
            playerActionMaps ??= new PlayerActionMaps();
            playerActionMaps.Player.Enable();
            playerActionMaps.Player.Fire.performed += HandleHire;
        }

        private void OnDestroy()
        {
            playerActionMaps.Disable();
        }

        private void Update()
        {
            var movementVector = playerActionMaps.Player.Move.ReadValue<Vector2>() * Time.deltaTime * moveSpeed;
            transform.Translate(new Vector3(movementVector.x, 0, movementVector.y));
        }

        private void HandleHire(InputAction.CallbackContext context)
        {
            Debug.Log($"Fire - {context.phase}");
        }
    }
}
