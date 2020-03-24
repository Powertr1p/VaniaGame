using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Config")]
    [SerializeField] private float _movementSpeed = 6f;
    [SerializeField] private float _jumpVelocity;
    private float _dashSpeed = 20f;
    private float _dashCooldown = 2f;
    private float _dashingTime = 0.15f;

    private float _originalMovementSpeedValue;

    private bool _canDoubleJump;
    private bool _isGrounded;
    private bool _canDash = true;
    private bool _isDashing;

    private PlayerState _player;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private BoxCollider2D _feetCollider;

    private bool _isRunning => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;

    public bool CanMove => _player.IsAlive;

    private void Awake()
    {
        _player = GetComponent<PlayerState>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _feetCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _originalMovementSpeedValue = _movementSpeed;
    }

    private void FixedUpdate()
    {
        _isGrounded = _feetCollider.IsTouchingLayers(LayerMask.GetMask(Constants.Ground));

        if (_isGrounded)
            _canDoubleJump = true;
    }

    public void TryMove(float direction)
    {
        if (_isDashing) return;

        _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction);

        SwapSpriteFacing(direction);

        _animator.SetBool(Constants.Running, _isRunning);
    }
    private void SwapSpriteFacing(float direction)
    {
        if (_isRunning)
            transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }

    private Vector2 GetPlayerVelocityBasedOnDirection(float direction)
    {
        return new Vector2(direction * _movementSpeed, _rb2d.velocity.y);
    }

    public IEnumerator TryDash(float direction)
    {
        if (!_canDash || _isDashing) yield return null;

        _canDash = false;
        _isDashing = true;

        _movementSpeed += _dashSpeed;
        _rb2d.gravityScale = 0;
        _rb2d.velocity = Vector2.zero;

        PerformDash(transform.localScale.x);
        yield return new WaitForSeconds(_dashingTime);

        StartCoroutine(TryStopDash());
    }

    private void PerformDash(float direction)
    {
        _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction);
    }

    private IEnumerator TryStopDash()
    {
        _rb2d.gravityScale = 1;
        _movementSpeed = _originalMovementSpeedValue;
        _isDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    public void TryJump()
    {
        if (_isGrounded)
        {
            _canDoubleJump = true;
            _rb2d.velocity = Vector2.up * _jumpVelocity;
        }
        else if (_canDoubleJump && !_isGrounded)
        {
            _rb2d.velocity = Vector2.up * _jumpVelocity;
            _canDoubleJump = false;
        }
    }
}
