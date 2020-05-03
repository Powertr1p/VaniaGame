using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInput : MonoBehaviour
{
    public event Action OnJumpButtonPressed;
    public event Action<float> OnMovementButtonPressed;
    public event Action<float> OnDashButtonPressed;

    private void Update()
    {
        InputDirectionStorage.StoreLastNonZeroDirection(CrossPlatformInputManager.GetAxisRaw("Horizontal"));
        
        OnMovementButtonPressed?.Invoke(InputDirectionStorage.CurrentDirection);

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            OnJumpButtonPressed?.Invoke();

        if (CrossPlatformInputManager.GetButtonDown("Shift"))
            OnDashButtonPressed?.Invoke(InputDirectionStorage.LastNonZeroDirection);
    }
}
