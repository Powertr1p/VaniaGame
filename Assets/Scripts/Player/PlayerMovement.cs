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
    [SerializeField]  private int _amountOfJumps = 2;
    [Header("Dash config")]
    [Tooltip("Скорость дэша, которая прибавляется к movementSpeed игрока для рывка.")]
    [SerializeField] private float _dashSpeed = 20f;
    [Tooltip("Кулдаун дэша")]
    [SerializeField] private float _dashCooldown = 2f;
    [Tooltip("Время перфоманса дэша, советуется указывать в сотых")]
    [SerializeField] private float _dashingTime = 0.15f;
    [Header("Debug panel for GameDesigners")]
    public Vector2 CurrentPlayerVelocity; //удалить после того как ГД настроят все

    [SerializeField] private Vector2 _dashVelocity;

    private float _originalMovementSpeedValue;
    private int _originalAmountOfJumps;
    

    private bool _canDoubleJump;
    private bool _canDash = true;
    private bool _isDashing;
    private bool _canWallJump;

    private PlayerState _player;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private Collisions _collisions;
    private PlayerInput _input;
    [SerializeField] private GameObject _trail;

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
        _input.OnDashButtonPressed += Dash;
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
        _originalAmountOfJumps = _amountOfJumps;
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
            RestoreJump();
        }

        if (_collisions.IsJumpPad)
            _canDoubleJump = true;

        if (_collisions.IsOnWallAndReadyToWallJump)
            WallSlide();

        if (_isDashing)
           StartCoroutine(OldDash(InputDirectionStorage.LastNonZeroDirection));

        if (_collisions.IsOnWall)
            RestoreJump();

        if (!_collisions.IsGrounded)
            ChangeGravityOnFall();
    }

    private void ChangeGravityOnFall()
    {
        if (_rb2d.velocity.y < 0)
            _rb2d.gravityScale = _jumpFallingGravity;
    }

    private void TryMove(float direction)
    {
        if (!_canMove) return;
        
        if (!_isDashing || !_collisions.IsOnWallAndReadyToWallJump)
        {
            _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction, _movementSpeed);
            _animator.SetBool(Constants.Running, IsRunning()); //TODO: вывести в отдельный компонент
        }
    }

    private void TryJump()
    {
        if (!_canMove) return;

        // if (_collisions.IsGrounded)
        //     Jump(true);
        // else if (_canDoubleJump && !_collisions.IsGrounded)
        //     Jump(false);
        
        Jump();
    }

    private void Jump()
    {
        if (_amountOfJumps < 1) return;
        
        _amountOfJumps--;
        _rb2d.gravityScale = 1.2f;
        _rb2d.velocity = Vector2.up * _jumpVelocity;
    }

    private Vector2 GetPlayerVelocityBasedOnDirection(float direction, float movementSpeed)
    {
        return new Vector2(direction * movementSpeed, _rb2d.velocity.y);
    }

    // private void ToggleDash(float direction)
    // {
    //     if (_canDash)
    //     {
    //         _canDash = false;
    //         _isDashing = true;
    //     }
    // }

    private void Dash(float direction)
    {
        var trailObject = Instantiate(_trail, transform.position, Quaternion.identity);
        trailObject.transform.SetParent(transform);
        Destroy(trailObject, 0.5f);
        
        _rb2d.AddForce(_dashVelocity * new Vector2(direction, 1), ForceMode2D.Impulse);
    }

    private IEnumerator OldDash(float direction)
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

    private void RestoreJump()
    {
         if (_amountOfJumps < 1)
             _amountOfJumps = _originalAmountOfJumps;
    }

    private void OnDisable()
    {
        _input.OnJumpButtonPressed -= TryJump;
        _input.OnMovementButtonPressed -= TryMove;
        _input.OnDashButtonPressed -= Dash;
    }
}
