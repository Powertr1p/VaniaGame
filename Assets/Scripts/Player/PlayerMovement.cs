using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public event Action OnWallslide;
    public event Action OnDashing;
    public event Action OnAttemptToDash;
    
    [Header("Player Config")]
    [Tooltip("Скорость, с которой двигается игрок, эта скорость также влияет на скорость дэша")]
    [SerializeField] private float _movementSpeed = 6f;
    [Tooltip("Сила прыжка. Придает игроку горизонтальный пуш (считается так: координата Х берется из инпута, а этот параметр умножает Y")]
    [SerializeField] private float _jumpVelocity;
    [Tooltip("После прыжка на игрока дейтсвует усиленная гравитация для эффекта тяжести прыжка")]
    [SerializeField] private float _jumpFallingGravity = 2f;
    [FormerlySerializedAs("_amountOfJumps")] [SerializeField] public int AmountOfJumps = 2;

    [Header("Dash config")] 
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashCooldown;
    private float _dashTimeLeft;
    private float _lastDash = -100f;
    private float _direction;

    [Header("Debug panel for GameDesigners")]
    public Vector2 CurrentPlayerVelocity;
    
    private int _originalAmountOfJumps;
    
    [SerializeField] private bool _canWallClimb = true;
    [SerializeField] private bool _canDash = true;
    private bool _isDashing;
    private bool _restoringJump;
    
    private PlayerState _player;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private Collisions _collisions;
    private PlayerInput _input;

    private bool CanMove => _player.IsAlive;

    public bool IsRunning() => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;

    private void Awake()
    {
        _player = GetComponent<PlayerState>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collisions = GetComponent<Collisions>();
        _input = GetComponent<PlayerInput>();
    }
    
    private void OnEnable()
    {
        _input.OnJumpButtonPressed += TryJump;
        _input.OnMovementButtonPressed += TryMove;
        _input.OnDashButtonPressed += TryDash;
    }

    private void Start()
    {
        _originalAmountOfJumps = AmountOfJumps;
    }
    
    private void FixedUpdate()
    {
        CurrentPlayerVelocity = _rb2d.velocity;

        if (_collisions.IsGrounded)
            _rb2d.gravityScale = 1f;

        if (_collisions.IsWallslide)
            WallSlide();

        if (!_collisions.IsGrounded)
            ChangeGravityOnFall();

        PerformDash();
        TryRestoreJump();
    }

    private void TryDash(float direction)
    {
        if (!_canDash) return;
        
        _direction = direction;
        
        if (Time.time >= (_lastDash + _dashCooldown))
            AttemptToDash();
    }
    
    private void PerformDash()
    {
        if (!_isDashing) return;
        
        if (_dashTimeLeft > 0)
        {
            _rb2d.velocity = Vector3.zero;
            _rb2d.gravityScale = 0f;
            _rb2d.velocity = new Vector2(_dashSpeed * _direction, _rb2d.velocity.y);
            _dashTimeLeft -= Time.deltaTime;

            OnDashing?.Invoke();
        }
        else if (_dashTimeLeft <= 0)
        {
            _rb2d.gravityScale = 1f;
            _isDashing = false;
        }
    }
    
    private void AttemptToDash()
    {
        _isDashing = true;
        _dashTimeLeft = _dashTime; 
        _lastDash = Time.time;

        OnAttemptToDash?.Invoke();
    }

    private void ChangeGravityOnFall()
    {
        if (_rb2d.velocity.y < 0)
            _rb2d.gravityScale = _jumpFallingGravity;
    }

    private void TryMove(float direction)
    {
        if (!CanMove) return;
        
        _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction, _movementSpeed);
        _animator.SetBool(Constants.Running, IsRunning());
    }

    private void TryJump()
    {
        if (!CanMove) return;
        Jump();
    }

    private void Jump()
    {
        if (AmountOfJumps < 1) return;

        AmountOfJumps--;
        _rb2d.gravityScale = 1f;
        _rb2d.velocity = Vector2.up * _jumpVelocity;
    }

    private Vector2 GetPlayerVelocityBasedOnDirection(float direction, float movementSpeed)
    {
        return new Vector2(direction * movementSpeed, _rb2d.velocity.y);
    }
 
    private void WallSlide()
    {
        if (!_canWallClimb) return;
        
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, -1);
        OnWallslide?.Invoke();
    }
    
    private void TryRestoreJump()
    {
        if (_restoringJump) return;

        StartCoroutine(RestoreJump());
    }
    
    private IEnumerator RestoreJump()
    {
        _restoringJump = true;
        
        if (AmountOfJumps < _originalAmountOfJumps && !_collisions.IsGrounded)
        {
            yield return new WaitUntil(() => _collisions.IsGrounded || _collisions.IsOnWall || _collisions.IsJumpPad );
           
            if (_collisions.IsOnRightWall && _canWallClimb)
            {
                AmountOfJumps = 1;
                yield return new WaitUntil(() => _collisions.IsOnLeftWall || _collisions.IsGrounded);
                
            }
            else if (_collisions.IsOnLeftWall && _canWallClimb)
            {
                AmountOfJumps = 1;
                yield return new WaitUntil(() => _collisions.IsOnRightWall || _collisions.IsGrounded);
            }
            
            if (_collisions.IsGrounded)
                AmountOfJumps = _originalAmountOfJumps;
        }
        _restoringJump = false;
    }

    private void OnDisable()
    {
        _input.OnJumpButtonPressed -= TryJump;
        _input.OnMovementButtonPressed -= TryMove;
        _input.OnDashButtonPressed -= TryDash;
    }
}
