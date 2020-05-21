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
    private float _lastImageXPos;
    private float _distanceBetweenImages = 0.1f;
    
    [Header("Debug panel for GameDesigners")]
    public Vector2 CurrentPlayerVelocity;
    
    private int _originalAmountOfJumps;
    
    private bool _canWallJump;
    private bool _isDashing;
    private bool _restoringJump;
    
    private PlayerState _player;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private Collisions _collisions;
    private PlayerInput _input;

    private bool _canMove => _player.IsAlive;

    public bool IsRunning() => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;

    private void OnEnable()
    {
        _input.OnJumpButtonPressed += TryJump;
        _input.OnMovementButtonPressed += TryMove;
        _input.OnDashButtonPressed += TryDash;
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
        
        CheckDash();
        TryRestoreJump();
    }

    private void TryDash(float direction)
    {
        _direction = direction;
        
        if (Time.time >= (_lastDash + _dashCooldown))
            AttemptToDash();
    }
    
    private void CheckDash()
    {
        if (!_isDashing) return;
        
        if (_dashTimeLeft > 0)
        {
            _rb2d.velocity = Vector3.zero;
            _rb2d.gravityScale = 0f;
            _rb2d.velocity = new Vector2(_dashSpeed * _direction, _rb2d.velocity.y);
            _dashTimeLeft -= Time.deltaTime;

            if (Mathf.Abs(transform.position.x - _lastImageXPos) > _distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                _lastImageXPos = transform.position.x;
            }
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

        PlayerAfterImagePool.Instance.GetFromPool();
        _lastImageXPos = transform.position.x;
    }

    private void ChangeGravityOnFall()
    {
        if (_rb2d.velocity.y < 0)
            _rb2d.gravityScale = _jumpFallingGravity;
    }

    private void TryMove(float direction)
    {
        if (!_canMove) return;
        
        _rb2d.velocity = GetPlayerVelocityBasedOnDirection(direction, _movementSpeed);
        _animator.SetBool(Constants.Running, IsRunning());
    }

    private void TryJump()
    {
        if (!_canMove) return;
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
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, -1);
    }

    private void TryRestoreJump()
    {
        if (_collisions.IsGrounded && _collisions.IsOnWall)
            AmountOfJumps = _originalAmountOfJumps;
        
        if (_restoringJump) return;
        
        StartCoroutine(RestoreJump());
    }
    
    private IEnumerator RestoreJump()
    {
        _restoringJump = true;
        
        if (AmountOfJumps < _originalAmountOfJumps && !_collisions.IsGrounded)
        {
            yield return new WaitUntil(() => _collisions.IsGrounded || _collisions.IsOnWall || _collisions.IsJumpPad );
           
            if (_collisions.IsOnWall)
            {
                AmountOfJumps += 1;
                UnityEngine.Debug.Log("amount +1");
            }
            else if (_collisions.IsGrounded) 
                AmountOfJumps = _originalAmountOfJumps;
           
            yield return new WaitUntil(() => !(_collisions.IsGrounded || _collisions.IsOnWall || _collisions.IsJumpPad));
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
