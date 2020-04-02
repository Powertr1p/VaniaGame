using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Collisions : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private Vector2 _bottomOffset;
    [SerializeField] private Vector2 _rightOffset;
    [SerializeField] private Vector2 _leftOffset;
    [SerializeField] private float _collisionRadius;
    
    private float _wallSlideResidualCollisionTimer = 0.3f;
    private float _wallSlideResidualCollisionTimerValue;
   
    private bool _isGrounded;
    private bool _isOnLeftWall;
    private bool _isOnRightWall;
    private bool _isOnWall;

    private Rigidbody2D _rb2d;
    private float _facingDirection;

    public bool IsGrounded { get => _isGrounded; }
    public bool IsOnWall { get => _isOnWall; }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _wallSlideResidualCollisionTimerValue = _wallSlideResidualCollisionTimer;
    }

    private void FixedUpdate()
    {
        _isGrounded = IsCollided((Vector2)transform.position + _bottomOffset, _collisionRadius, _groundLayer);
        _isOnRightWall = IsCollided((Vector2)transform.position + _rightOffset, _collisionRadius, _groundLayer);
        _isOnLeftWall = IsCollided((Vector2)transform.position + _leftOffset, _collisionRadius, _groundLayer);

        _facingDirection = transform.localScale.x;

        CheckForWallSlide();
        
        if (_wallSlideResidualCollisionTimer < 0)
            TryResetWallSlideCollisionTimer();
    }

    private void CheckForWallSlide() //первичный прототип на коленке, надо отрефакторить нормально
    {
        if (_isOnRightWall && _facingDirection == 1 && _rb2d.velocity.y < 0 && !_isGrounded)
        {
            if (_rb2d.velocity.x > 0)
            {
                _isOnWall = true;
            }
            else if (_rb2d.velocity.x == 0 && _wallSlideResidualCollisionTimer > 0)
            {
                ToggleResidualCollisionAndStartTimer();
            }
            else
            {
                _isOnWall = false;
            }
        }
        else if (_isOnLeftWall && _facingDirection == -1 && _rb2d.velocity.y < 0 && !_isGrounded)
        {
            if (_rb2d.velocity.x < 0)
            {
                _isOnWall = true;
            }
            else if (_rb2d.velocity.x == 0 && _wallSlideResidualCollisionTimer > 0)
            {
                ToggleResidualCollisionAndStartTimer();
            }
            else
            {
                _isOnWall = false;
            }
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

    private void ToggleResidualCollisionAndStartTimer()
    {
        ReduceWallSliderCollisionTimer();
        _isOnWall = true;
    }

    private void ReduceWallSliderCollisionTimer()
    {
        if (_wallSlideResidualCollisionTimer >= 0)
            _wallSlideResidualCollisionTimer -= Time.deltaTime;

        Debug.Log(_wallSlideResidualCollisionTimer);
    }

    private void TryResetWallSlideCollisionTimer() //прототип
    {
        if (!_isOnLeftWall && !_isOnRightWall || _isGrounded)
            _wallSlideResidualCollisionTimer = _wallSlideResidualCollisionTimerValue;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + _bottomOffset, _collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + _rightOffset, _collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + _leftOffset, _collisionRadius);
    }
}
