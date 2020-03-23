using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;

    private void Update()
    {
        if (!_movement.CanMove) return;

        float direction = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        _movement.TryMove(direction);

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            _movement.TryJump();

        if (CrossPlatformInputManager.GetButtonDown("Shift"))
            _movement.TryDash();

        if (CrossPlatformInputManager.GetButtonUp("Shift"))
            _movement.StopDash();
    }
}
