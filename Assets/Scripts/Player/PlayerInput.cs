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
        InputDirectionStorage.StoreLastNonZeroDirection(CrossPlatformInputManager.GetAxisRaw("Horizontal"));
        
        OnMovementButtonPressed?.Invoke(InputDirectionStorage.CurrentDirection);

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            OnJumpButtonPressed?.Invoke(InputDirectionStorage.CurrentDirection);

        if (CrossPlatformInputManager.GetButtonDown("Shift"))
            OnDashButtonPressed?.Invoke(InputDirectionStorage.LastNonZeroDirection);
    }
}
