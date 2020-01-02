using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpSpeed = 11f;

    private Rigidbody2D _rb2d;
    private Animator _animator;

    

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }

    private void Movement()
    {
        float direction = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(direction * _speed, _rb2d.velocity.y);
        _rb2d.velocity = playerVelocity;
        SwapFacing(direction);
        bool isRunning = Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;
        _animator.SetBool("Running", isRunning);
    }

    private void SwapFacing(float direction)
    {
        bool playerHorizontalSpeed = Mathf.Abs(_rb2d.velocity.x) > Mathf.Epsilon;
        if (playerHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0f, _jumpSpeed);
            _rb2d.velocity += jumpVelocity;
        }
    }


}
