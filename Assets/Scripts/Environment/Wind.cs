using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private Vector2 _windForce;

    private Rigidbody2D _playerRigidbody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerRigidbody == null) return;
        
        _playerRigidbody.AddForce(_windForce);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerRigidbody = null;
    }
}
