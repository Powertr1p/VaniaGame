using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;

    private void Update()
    {
        float direction = CrossPlatformInputManager.GetAxisRaw("Horizontal");

    }
}
