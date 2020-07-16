using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour, ITriggerable
{
    [SerializeField] private Vector2 _windForce;
    [SerializeField] private bool _isActivated;

    private ParticleSystem _particle;
    private Rigidbody2D _playerRigidbody;

    private void Awake()
    {
        _particle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        if (_isActivated)
            _particle.Play();
        else
            _particle.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerRigidbody == null || !_isActivated) return;
        
        _playerRigidbody.AddForce(_windForce);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerRigidbody = null;
    }

    public void Activate()
    {
        _isActivated = true;
        _particle.Play();
    }

    public void Deactivate()
    {
        _isActivated = false;
        _particle.Stop();
    }
}
