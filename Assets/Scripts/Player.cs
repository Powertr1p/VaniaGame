using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpSpeed = 20f;
    [SerializeField] private float _climbSpeed = 5f;
    [SerializeField] private Vector2 _deathKick;

    private float _normalGravityScale;

    private bool IsRunning => Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;
    public bool IsAlive = true;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _feetCollider;

    #region CONST_STRINGS
    private const string _runningAnimation = "Running";
    private const string _climbingAnimation = "Climbing";
    private const string _diedAnimation = "Died";
    private const string _ladderLayer = "Ladder";
    private const string _enemyLayer = "Enemy";
    private const string _groundLayer = "Ground";
    private const string _hazardsLayer = "Hazards";
    #endregion

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _normalGravityScale = _rb2d.gravityScale;
    }

    private void Update()
    {
        if (!IsAlive) { return; }
        
        Movement();
        Jump();
        Climbing();
        if (CheckDieConditions())
            Die();
    }

    private void Movement()
    {
        float direction = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        Vector2 playerVelocity = new Vector2(direction * _speed, _rb2d.velocity.y);
        _rb2d.velocity = playerVelocity;

        SwapSpriteFacing(direction);

        _animator.SetBool(_runningAnimation, IsRunning);
    }

    private void SwapSpriteFacing(float direction)
    {
        if (IsRunning)
            transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }

    private void Jump()
    {
        if (!_feetCollider.IsTouchingLayers(LayerMask.GetMask(_groundLayer))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0F, _jumpSpeed);
            _rb2d.velocity += jumpVelocity;
        }
    }

    private void Climbing()
    {
        if (!_feetCollider.IsTouchingLayers(LayerMask.GetMask(_ladderLayer)))
        {
            _rb2d.gravityScale = _normalGravityScale;
            _animator.SetBool(_climbingAnimation, false);
            return;
        }

        _rb2d.gravityScale = 0F;
        float direction = CrossPlatformInputManager.GetAxisRaw("Vertical");
        Vector2 climbVelocity = new Vector2(_rb2d.velocity.x, direction * _climbSpeed);
        _rb2d.velocity = climbVelocity;

        bool isMovingVertical = Mathf.Abs(_rb2d.velocity.y) > Mathf.Epsilon;
        _animator.SetBool(_climbingAnimation, isMovingVertical);
    }

    private bool CheckDieConditions()
    {
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask(_enemyLayer)))
            return true;
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask(_hazardsLayer)))
            return true;
        else
            return false;
    }

    private void Die()
    {
       IsAlive = false;
       _animator.SetTrigger(_diedAnimation);
       _rb2d.velocity = _deathKick;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
