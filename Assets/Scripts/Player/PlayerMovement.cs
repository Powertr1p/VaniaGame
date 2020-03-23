using System;
using System.Collections;
using UnityEngine;

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
    [SerializeField] private float _dashSpeed = 6f;
    private float _originalMovementSpeedValue;

    private bool _canDoubleJump;
    private bool _isGrounded;

    private PlayerState _player;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private BoxCollider2D _feetCollider;

    private bool _isRunning => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;

    public bool CanMove => _player.IsAlive;

    private void Start()
    {
        _player = GetComponent<PlayerState>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _feetCollider = GetComponent<BoxCollider2D>();

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

    public void TryDash()
    {
        _movementSpeed += _dashSpeed;
    }

    public void StopDash()
    {
        _movementSpeed = _originalMovementSpeedValue;
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
