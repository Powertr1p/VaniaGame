using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Config")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _jumpSpeed = 2.5f;
    [SerializeField] private float _jumpTime = 0.11f;

    private bool _isJumping;
    private float _jumpTimeCounter;

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

    public void TryJump()
    {
        bool isGrounded = _feetCollider.IsTouchingLayers(LayerMask.GetMask(Constants.Ground));

        Vector2 jumpVelocity = new Vector2(0f, _jumpSpeed);

        if (isGrounded)
        {
            _isJumping = true;
            _jumpTimeCounter = _jumpTime;
            _rb2d.velocity += jumpVelocity;
        }

        if (_isJumping)
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
}
