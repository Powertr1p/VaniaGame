using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerSpriteFacingHandler : MonoBehaviour
{
    private PlayerMovement _movement;
    private bool CanSwapFacing() => _movement.IsRunning();

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (CanSwapFacing())
            SwapSpriteFacing(InputDirectionHandler.CurrentDirection);
    }

    private void SwapSpriteFacing(float direction)
    {
        if (direction != 0)
            transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }
}
