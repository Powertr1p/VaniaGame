using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInput : MonoBehaviour
{
    public event Action<float> OnJumpButtonPressed;
    public event Action<float> OnMovementButtonPressed;
    public event Action<float> OnDashButtonPressed;

    private void Update()
    {
        InputDirectionHandler.StoreLastNonZeroDirection(CrossPlatformInputManager.GetAxisRaw("Horizontal"));
        
        OnMovementButtonPressed?.Invoke(InputDirectionHandler.CurrentDirection);

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            OnJumpButtonPressed?.Invoke(InputDirectionHandler.CurrentDirection);

        if (CrossPlatformInputManager.GetButtonDown("Shift"))
            OnDashButtonPressed?.Invoke(InputDirectionHandler.LastNonZeroDirection);
    }
}
