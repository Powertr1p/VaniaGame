using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;

    private void Update()
    {
        if (!_movement.CanMove) return;

        InputDirectionHandler.StoreLastNonZeroDirection(CrossPlatformInputManager.GetAxisRaw("Horizontal"));
        _movement.TryMove(InputDirectionHandler.CurrentDirection);

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            _movement.TryJump(InputDirectionHandler.CurrentDirection);

        if (CrossPlatformInputManager.GetButtonDown("Shift"))
            StartCoroutine(_movement.TryDash(InputDirectionHandler.LastNonZeroDirection));
    }
}
