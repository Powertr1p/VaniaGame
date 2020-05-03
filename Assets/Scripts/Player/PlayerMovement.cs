using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public event Action Grounded;

    [Header("Player Config")]
    [Tooltip("Скорость, с которой двигается игрок, эта скорость также влияет на скорость дэша")]
    [SerializeField] private float _movementSpeed = 6f;
    [Tooltip("Сила прыжка. Придает игроку горизонтальный пуш (считается так: координата Х берется из инпута, а этот параметр умножает Y")]
    [SerializeField] private float _jumpVelocity;
    [Tooltip("После прыжка на игрока дейтсвует усиленная гравитация для эффекта тяжести прыжка")]
    [SerializeField] private float _jumpFallingGravity = 1.5f;
    [Header("Dash config")]
    [Tooltip("Скорость дэша, которая прибавляется к movementSpeed игрока для рывка.")]
    [SerializeField] private float _dashSpeed = 20f;
    [Tooltip("Кулдаун дэша")]
    [SerializeField] private float _dashCooldown = 2f;
    [Tooltip("Время перфоманса дэша, советуется указывать в сотых")]
    [SerializeField] private float _dashingTime = 0.15f;
    [Header("Walljump Config")]
    [Tooltip("Горизонтальная сила при отталкивании от стены. Придает силу физическому телу по координате Х.")]
    [SerializeField] private float _wallJumpHorizontalVelocity = 10f;
    [Tooltip("Вертикальная сила при отталкивании от стены. Придает силу физическому телу по координате Y.")]
    [SerializeField] private float _wallJumpVerticalVelocity = 700f;
    [Header("Debug panel for GameDesigners")]
    public Vector2 CurrentPlayerVelocity; //удалить после того как ГД настроят все

    private float _originalMovementSpeedValue;

    private bool _canDoubleJump;
    private bool _canDash = true;
    private bool _isDashing;
    private bool _canWallJump;

    private PlayerState _player;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private Collisions _collisions;
    private PlayerInput _input;

    private bool _canMove => _player.IsAlive;

    public bool IsRunning() => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;

    public IEnumerator RechargeDash()
    {
        yield return new WaitUntil(() => _isDashing == false);
        _canDash = true;
    }

    private void OnEnable()
    {
        _input.OnJumpButtonPressed += TryJump;
        _input.OnMovementButtonPressed += TryMove;
        _input.OnDashButtonPressed += ToggleDash;
    }

    private void Awake()
    {
        _player = GetComponent<PlayerState>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collisions = GetComponent<Collisions>();
        _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        _originalMovementSpeedValue = _movementSpeed;
    }

    private void FixedUpdate()
    {
        CurrentPlayerVelocity = _rb2d.velocity; //удалить после того как ГД настроят все

        if (_collisions.IsGrounded)
        {
            Grounded?.Invoke();
            _canDoubleJump = true;
            _rb2d.gravityScale = 1f;
        }

        if (_collisions.IsJumpPad)
            _canDoubleJump = true;

        if (_collisions.IsOnWallAndReadyToWallJump)
            WallSlide();

        if (_isDashing)
           StartCoroutine(Dash(InputDirectionStorage.LastNonZeroDirection));

        if (!_canWallJump)
            TryRestoreWallJump();

        if (!_collisions.IsGrounded)
            ChangeGravityOnFall();
    }

    private void ChangeGravityOnFall()
    {
        if (_rb2d.velocity.y < 0)
            _rb2d.gravityScale = _jumpFallingGravity;
    }

    private void TryMove(float direction) //TODO: вынести общёт физики в фиксед и сделать finite state machine уже наконец
    {
        if (!_canMove) return;
        
        if (!_isDashing || !_collisions.IsOnWallAndReadyToWallJump)
        {
            _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction, _movementSpeed);
            _animator.SetBool(Constants.Running, IsRunning()); //TODO: вывести в отдельный компонент
        }
    }

    private void TryJump(float direction)
    {
        if (!_canMove) return;

        if (_collisions.IsGrounded)
            Jump(true);
        else if (_canDoubleJump && !_collisions.IsGrounded && !_collisions.IsOnWall)
            Jump(false);
        else if (_canWallJump && (_collisions.IsOnWallAndReadyToWallJump || _collisions.IsOnWall))
            TryWallJump(direction);
    }

    private void Jump(bool canDoExtraJump)
    {
        _rb2d.gravityScale = 1.2f;
        _canWallJump = false;
        _canDoubleJump = canDoExtraJump;
        _rb2d.velocity = Vector2.up * _jumpVelocity;
    }

    private void TryWallJump(float direction) 
    {
        if (_canWallJump)
        {
            Vector2 force = new Vector2(_wallJumpHorizontalVelocity * direction, _wallJumpVerticalVelocity);
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(force);
            _canWallJump = false;
        }
    }

    private Vector2 GetPlayerVelocityBasedOnDirection(float direction, float movementSpeed)
    {
        return new Vector2(direction * movementSpeed, _rb2d.velocity.y);
    }

    private void ToggleDash(float direction)
    {
        if (_canDash)
        {
            _canDash = false;
            _isDashing = true;
        }
    }

    private IEnumerator Dash(float direction)
    {
        _rb2d.gravityScale = 0;
        _rb2d.velocity = Vector2.zero;
        _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction, _movementSpeed + _dashSpeed);
        yield return new WaitForSeconds(_dashingTime);
        StopDash();
        StartCoroutine(ActivateDashCooldown());
    }

    private void StopDash()
    {
        _rb2d.gravityScale = 1;
        _movementSpeed = _originalMovementSpeedValue;
        _isDashing = false;
    }

    private IEnumerator ActivateDashCooldown()
    {
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    private void WallSlide()
    {
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, -1);
    }

    private void TryRestoreWallJump()
    {
        if (!_collisions.IsOnWall || _collisions.IsGrounded)
            _canWallJump = true;
    }

    private void OnDisable()
    {
        _input.OnJumpButtonPressed -= TryJump;
        _input.OnMovementButtonPressed -= TryMove;
        _input.OnDashButtonPressed -= ToggleDash;
    }
}
