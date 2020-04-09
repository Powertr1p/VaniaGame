using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class JumpPad : MonoBehaviour, IInteractable
{
    [Header("JumpPad Config")]
    [Tooltip("Сила, с которой джампад выталкивает игрока вверх (от 500 и выше)")]
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _playerRigidBody;

    public void Interact()
    {
        if (_playerRigidBody == null) return;

        _playerRigidBody.velocity = Vector2.zero;
        _playerRigidBody.AddForce(new Vector2(0, _jumpForce));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _playerRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        
        Interact();
    }

    private void OnTriggerExit(Collider other)
    {
        _playerRigidBody = null;
    }
}
