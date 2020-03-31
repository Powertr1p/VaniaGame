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
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashCooldown = 2f;
    [SerializeField] private float _dashingTime = 0.15f;

    public event Action OnDirectionChanged;

    private float _originalMovementSpeedValue;

    private bool _canDoubleJump;
    private bool _canDash = true;
    private bool _isDashing;

    private PlayerState _player;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private Collisions _collisions;
    private float _lastJumpedDirection;

    public bool IsRunning() => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;

    public bool CanMove => _player.IsAlive;

    private void Awake()
    {
        _player = GetComponent<PlayerState>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collisions = GetComponent<Collisions>();
    }

    private void Start()
    {
        _originalMovementSpeedValue = _movementSpeed;
    }

    private void FixedUpdate()
    {
        if (_collisions.IsGrounded)
            _canDoubleJump = true;

        if (_collisions.IsOnWall)
            PerformWallSlide();
    }

    public void TryMove(float direction)
    {
        if (_isDashing || _collisions.IsOnWall) return;

        _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction, _movementSpeed);

        _animator.SetBool(Constants.Running, IsRunning()); //вывести в отдельный компонент

        Debug.Log(_rb2d.velocity.x);
    }

    public void TryJump(float direction)
    {
        if (_collisions.IsGrounded)
            Jump(true);
        else if (_canDoubleJump && !_collisions.IsGrounded && !_collisions.IsOnWall)
            Jump(false);
        else
            TryWallJump(direction);
    }

    private void Jump(bool canDoExtraJump)
    {
        _canDoubleJump = canDoExtraJump;
        _rb2d.velocity = Vector2.up * _jumpVelocity;
    }

    private void TryWallJump(float direction)  //первичный прототип, надо отрефакторить нормально
    {
        if (_collisions.IsOnWall && !_collisions.IsGrounded && direction == transform.localScale.x * -1) //убрать зависимость от трансформа, сделать более гибкой
        {
            Vector2 force = new Vector2(10f * direction, 700f); //вывести литералы в инспектор
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(force);
        }
    }

    private Vector2 GetPlayerVelocityBasedOnDirection(float direction, float movementSpeed)
    {
        return new Vector2(direction * movementSpeed, _rb2d.velocity.y);
    }

    public IEnumerator TryDash(float direction)
    {
        if (_canDash)
        { 
            _canDash = false;
            _isDashing = true;

            _rb2d.gravityScale = 0;
            _rb2d.velocity = Vector2.zero;
            _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction, _movementSpeed + _dashSpeed);
            yield return new WaitForSeconds(_dashingTime);

            StartCoroutine(StopDashAndActivateCooldown());
        }
    }

    private IEnumerator StopDashAndActivateCooldown()
    {
        _rb2d.gravityScale = 1;
        _movementSpeed = _originalMovementSpeedValue;
        _isDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    private void PerformWallSlide()
    {
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, -1);
    }
}
