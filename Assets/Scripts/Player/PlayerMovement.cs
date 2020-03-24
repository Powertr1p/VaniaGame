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
    private float _originalMovementSpeedValue;

    private bool _canDoubleJump;
    private bool _isGrounded;
    private bool _canDash = true;

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

    public IEnumerator TryDash(float direction)
    {
       if (_canDash)
        {
            _canDash = false;
            
            if (direction == 0)
            {
                
            }
            else
            {
                _movementSpeed += 20f;
                _rb2d.gravityScale = 0;
            }
        }

        yield return new WaitForSeconds(0.15f);
        StartCoroutine(TryStopDash());
    }

    private IEnumerator TryStopDash()
    {
        _rb2d.gravityScale = 1;
        _movementSpeed = _originalMovementSpeedValue;
        yield return new WaitForSeconds(2f);
        _canDash = true;
    }

    public void TryDash2(float direction)
    {
        _rb2d.velocity = Vector2.zero;
        Vector2 dashedVector = new Vector2(direction, 0);
        _rb2d.velocity += dashedVector.normalized * 100f;
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
