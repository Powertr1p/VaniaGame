using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpSpeed = 11f;
    [SerializeField] private float _climbSpeed = 5f;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private Collider2D _collider;
    

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        Climbing();
    }

    private void Movement()
    {
        float direction = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        Vector2 playerVelocity = new Vector2(direction * _speed, _rb2d.velocity.y);
        _rb2d.velocity = playerVelocity;

        SwapFacing(direction);

        _animator.SetBool("Running", IsRunning());
    }

    private void SwapFacing(float direction)
    {
        if (IsRunning())
            transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }

    private bool IsRunning()
    {
        return Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;
    }

    private void Jump()
    {
        if (!_collider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0f, _jumpSpeed);
            _rb2d.velocity += jumpVelocity;
        }
    }

    private void Climbing()
    {
        if (_collider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            float direction = CrossPlatformInputManager.GetAxisRaw("Vertical");
            Vector2 climbVelocity = new Vector2(_rb2d.velocity.x, direction * _climbSpeed);
            _rb2d.velocity = climbVelocity;

            bool isVerticalMoving = Mathf.Abs(_rb2d.velocity.y) > Mathf.Epsilon;
            _animator.SetBool("Climbing", isVerticalMoving);

        }
    }
}
