using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;

public class InputMovement : MonoBehaviour
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
    private PlayerWeapon _weapon;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private BoxCollider2D _feetCollider;

    public bool IsClimbing { get; private set; }
    private bool _isRunning => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;
    private bool _canMove => _player.IsAlive;

    #region CONST_STRINGS
    private const string _runningAnimation = "Running";
    private const string _climbingAnimation = "Climbing";
    private const string _climbingAnimationSpeed = "ClimbingSpeed";
    private const string _ladderLayer = "Ladder";
    private const string _groundLayer = "Ground";
    #endregion

    private void Start()
    {
        _player = GetComponent<Player>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _weapon = GetComponentInChildren<PlayerWeapon>();
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

        _animator.SetBool(_runningAnimation, _isRunning);
    }

    private void SwapSpriteFacing(float direction)
    {
        if (_isRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
            _weapon.transform.localScale = transform.localScale;
        }
    }

    private void Jump()
    {
        bool isGrounded = _feetCollider.IsTouchingLayers(LayerMask.GetMask(_groundLayer));
        bool isLadder = _feetCollider.IsTouchingLayers(LayerMask.GetMask(_ladderLayer));

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
                _jumpTimeCounter -= Time.deltaTime;
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
        if (!_feetCollider.IsTouchingLayers(LayerMask.GetMask(_ladderLayer)))
        {
            IsClimbing = false;
            _rb2d.gravityScale = _normalGravityScale;
            _animator.SetBool(_climbingAnimation, false);
            return;
        }

        _rb2d.gravityScale = 0F;
        float direction = CrossPlatformInputManager.GetAxisRaw("Vertical");
        Vector2 climbVelocity = new Vector2(_rb2d.velocity.x, direction * _climbSpeed);
        _rb2d.velocity = climbVelocity;
        _animator.SetBool(_climbingAnimation, true);
        IsClimbing = true;
        SwitchClimbingAnimation();
    }   

    private void SwitchClimbingAnimation()
    {
        bool isMovingVertical = Mathf.Abs(_rb2d.velocity.y) > Mathf.Epsilon;
        int animationSpeed = isMovingVertical ? 1 : 0;
        _animator.SetFloat(_climbingAnimationSpeed, animationSpeed);
    }

    private void Attack()
    {
        OnAttack?.Invoke();
    }
}
