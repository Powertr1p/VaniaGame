using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private Vector2 _bottomOffset;
    [SerializeField] private Vector2 _rightOffset;
    [SerializeField] private Vector2 _leftOffset;
    [SerializeField] private float _collisionRadius = 0.25f;

    private bool _isGrounded;
    private bool _isOnLeftWall;
    private bool _isOnRightWall;
    private bool _isOnWall;
    private float _facingDirection;

    private Rigidbody2D _rb2d;

    public bool IsGrounded { get => _isGrounded; }
    public bool IsOnWall { get => _isOnWall; }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _isGrounded = IsCollided((Vector2)transform.position + _bottomOffset, _collisionRadius, _groundLayer);

        _isOnRightWall = IsCollided((Vector2)transform.position + _rightOffset, _collisionRadius, _groundLayer);
        _isOnLeftWall = IsCollided((Vector2)transform.position + _leftOffset, _collisionRadius, _groundLayer);

        _facingDirection = transform.localScale.x;

        CheckForWallSlide();
    }

    private void CheckForWallSlide() //первичный прототип, надо отрефакторит нормально
    {
        if (_isOnRightWall && _facingDirection == 1 && _rb2d.velocity.y < 0 && !_isGrounded)
        {
            if (_rb2d.velocity.y > 0) return;
            _isOnWall = true;
        }
        else if (_isOnLeftWall && _facingDirection == -1 && _rb2d.velocity.y < 0 && !_isGrounded)
        {
            if (_rb2d.velocity.y > 0) return;
            _isOnWall = true;
        }
        else
        {
            _isOnWall = false;
        }
    }

    private bool IsCollided(Vector2 position, float collisionRadius, LayerMask layer)
    {
        return Physics2D.OverlapCircle(position, collisionRadius, layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + _bottomOffset, _collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + _rightOffset, _collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + _leftOffset, _collisionRadius);
    }
}
