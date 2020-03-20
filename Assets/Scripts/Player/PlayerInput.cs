using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    public UnityAction OnAttack;

    [Header("Player Config")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _jumpSpeed = 2.5f;
    [SerializeField] private float _jumpTime = 0.11f;
    [SerializeField] private float _climbSpeed = 5f;
    private bool _isJumping;
    private float _normalGravityScale;
    private float _jumpTimeCounter;

    private Player _player;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private BoxCollider2D _feetCollider;

    public bool IsClimbing { get; private set; }
    private bool _isRunning => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;
    private bool _canMove => _player.IsAlive;

    private void Start()
    {
        _player = GetComponent<Player>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _normalGravityScale = _rb2d.gravityScale;
    }

    private void Update()
    {
        if (!_canMove) { return; }

        Movement();
        Jump();
        Climbing();

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            Attack();
    }

    private void Movement()
    {
        float direction = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        Vector2 playerVelocity = new Vector2(direction * _movementSpeed, _rb2d.velocity.y);
        _rb2d.velocity = playerVelocity;

        SwapSpriteFacing(direction);

        _animator.SetBool(Constants.Running, _isRunning);
    }

    private void SwapSpriteFacing(float direction)
    {
        if (_isRunning)
            transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }

    private void Jump()
    {
        bool isGrounded = _feetCollider.IsTouchingLayers(LayerMask.GetMask(Constants.Ground));
        bool isLadder = _feetCollider.IsTouchingLayers(LayerMask.GetMask(Constants.Ladder));

        Vector2 jumpVelocity = new Vector2(0f, _jumpSpeed);

        if (CrossPlatformInputManager.GetButtonDown("Jump") && (isGrounded || isLadder))
        {
            _isJumping = true;
            _jumpTimeCounter = _jumpTime;
            _rb2d.velocity += jumpVelocity;
        }

        if (CrossPlatformInputManager.GetButton("Jump") && _isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                _rb2d.velocity += jumpVelocity;
                _jumpTimeCounter -= Time.fixedDeltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        if (CrossPlatformInputManager.GetButtonUp("Jump"))
            _isJumping = false;
    }

    private void Climbing()
    {
        if (!_feetCollider.IsTouchingLayers(LayerMask.GetMask(Constants.Ladder)))
        {
            EndClimbing();
            return;
        }

        StartClimbing();
        SwitchClimbingAnimation();
    }

    private void EndClimbing()
    {
        IsClimbing = false;
        _rb2d.gravityScale = _normalGravityScale;
        _animator.SetBool(Constants.Climbing, false);
    }

    private void StartClimbing()
    {
        _rb2d.gravityScale = 0F;
        float direction = CrossPlatformInputManager.GetAxisRaw("Vertical");
        Vector2 climbVelocity = new Vector2(_rb2d.velocity.x, direction * _climbSpeed);
        _rb2d.velocity = climbVelocity;
        _animator.SetBool(Constants.Climbing, true);
        IsClimbing = true;
    }

    private void SwitchClimbingAnimation()
    {
        bool isMovingVertical = Mathf.Abs(_rb2d.velocity.y) > Mathf.Epsilon;
        int animationSpeed = isMovingVertical ? 1 : 0;
        _animator.SetFloat(Constants.ClimbingSpeed, animationSpeed);
    }

    private void Attack()
    {
        OnAttack?.Invoke();
    }
}
