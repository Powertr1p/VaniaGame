﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.2F;
    private Rigidbody2D _rb2d;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 velocity = new Vector2(_speed, _rb2d.velocity.y);
        _rb2d.velocity = velocity;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MoveOppositeSide();
    }

    private void MoveOppositeSide()
    {
        transform.localScale = new Vector2(-_rb2d.velocity.x, 1F);
        _speed = -_speed;
    }
}
