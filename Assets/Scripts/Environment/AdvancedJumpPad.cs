using UnityEngine;

public class AdvancedJumpPad : JumpPad
{
    private Vector2 _lastHighestPlayerVelocity = Vector2.zero;
    private PlayerMovement _playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_lastHighestPlayerVelocity == Vector2.zero)
        {
            _playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            _playerMovement.Grounded += ResetLastHighestPlayerVelocity;
        }

        var currentVelocity = other.gameObject.GetComponent<Rigidbody2D>().velocity * -1;
        _lastHighestPlayerVelocity = CompareVelocityAndReturnHighest(currentVelocity);
    }

    public override void Interact()
    {
        if (PlayerRigidBody == null) return;

        PlayerRigidBody.velocity = Vector2.zero;
        PlayerRigidBody.AddForce(_lastHighestPlayerVelocity, ForceMode2D.Impulse);
    }

    private Vector2 CompareVelocityAndReturnHighest(Vector2 currentVelocity)
    {
        return currentVelocity.y > _lastHighestPlayerVelocity.y ? currentVelocity : _lastHighestPlayerVelocity;
    }

    private void ResetLastHighestPlayerVelocity()
    {
        _lastHighestPlayerVelocity = Vector2.zero;
        _playerMovement.Grounded -= ResetLastHighestPlayerVelocity;
    }
}
