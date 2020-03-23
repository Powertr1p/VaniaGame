﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerState : MonoBehaviour
{
    [SerializeField] private Vector2 _deathKick;

    public bool IsAlive = true;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private CapsuleCollider2D _bodyCollider;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (IsKilled() && IsAlive)
            Die();
    }

    private bool IsKilled()
    {
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask(Constants.Enemy)))
            return true;
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask(Constants.Hazards)))
            return true;
        else
            return false;
    }

    private void Die()
    {
        IsAlive = false;
        _animator.SetTrigger(Constants.Died);
        _rb2d.velocity = _deathKick;

        FindObjectOfType<GameSession>().PlayerDeath();
    }
}