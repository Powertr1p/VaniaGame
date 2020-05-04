using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Collisions : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _jumpPadLayer;

    [SerializeField] private Vector2 _bottomOffset;
    [SerializeField] private Vector2 _rightOffset;
    [SerializeField] private Vector2 _leftOffset;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private float _bottomCollisionRadius;
    
    [SerializeField] private float _wallSlideResidualCollisionTimer = 0.3f;
    private float _wallSlideResidualCollisionTimerValue;
   
    private bool _isGrounded;
    private bool _isOnLeftWall;
    private bool _isOnRightWall;
    private bool _isJumpPad;

    private Rigidbody2D _rb2d;
    private float _facingDirection;

    public bool IsGrounded { get => _isGrounded; }
    public bool IsOnWall { get => _isOnRightWall || _isOnLeftWall; }
    public bool IsJumpPad { get => _isJumpPad; }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _wallSlideResidualCollisionTimerValue = _wallSlideResidualCollisionTimer;
    }

    private void FixedUpdate()
    {
        _isGrounded = IsCollided((Vector2)transform.position + _bottomOffset, _bottomCollisionRadius, _groundLayer);
        _isOnRightWall = IsCollided((Vector2)transform.position + _rightOffset, _collisionRadius, _groundLayer);
        _isOnLeftWall = IsCollided((Vector2)transform.position + _leftOffset, _collisionRadius, _groundLayer);
        _isJumpPad = IsCollided((Vector2)transform.position + _bottomOffset, _bottomCollisionRadius, _jumpPadLayer);

        _facingDirection = transform.localScale.x;

        CheckWallslide();

        if (_wallSlideResidualCollisionTimer < 0)
            TryResetWallSlideCollisionTimer();
    }

    public bool CheckWallslide()
    {
        return IsPlayerCollidedWithWallFromLeftSide() || IsPlayerCollidedWithWallFromRightSide();
    }

    private bool IsPlayerCollidedWithWallFromRightSide() //отрефакторить
    {
        if (_isOnRightWall && _facingDirection == 1 && _rb2d.velocity.y < 0 && !_isGrounded)
        {
            if (_rb2d.velocity.x > 0)
            {
                return true;
            }
            else if (_rb2d.velocity.x == 0 && _wallSlideResidualCollisionTimer > 0)
            {
                ReduceResidualWallSlideCollisionTimer();
                return true;
            }
        }
        return false;
    }

    private bool IsPlayerCollidedWithWallFromLeftSide() //отрефакторить
    {
        if (_isOnLeftWall && _facingDirection == -1 && _rb2d.velocity.y < 0 && !_isGrounded)
        {
            if (_rb2d.velocity.x < 0)
            {
                return true;
            }
            else if (_rb2d.velocity.x == 0 && _wallSlideResidualCollisionTimer > 0)
            {
                ReduceResidualWallSlideCollisionTimer();
                return true;
            }
        }
        return false;
    }

    private bool IsCollided(Vector2 position, float collisionRadius, LayerMask layer)
    {
        return Physics2D.OverlapCircle(position, collisionRadius, layer);
    }

    private void ReduceResidualWallSlideCollisionTimer()
    {
        if (_wallSlideResidualCollisionTimer >= 0)
            _wallSlideResidualCollisionTimer -= Time.deltaTime;
    }

    private void TryResetWallSlideCollisionTimer()
    {
        if ((!_isOnLeftWall && !_isOnRightWall) || _isGrounded)
            _wallSlideResidualCollisionTimer = _wallSlideResidualCollisionTimerValue;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + _bottomOffset, _bottomCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + _rightOffset, _collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + _leftOffset, _collisionRadius);
    }
}
