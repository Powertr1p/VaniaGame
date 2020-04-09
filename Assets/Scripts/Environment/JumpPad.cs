using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("JumpPad Config")]
    [Tooltip("Сила, с которой джампад выталкивает игрока вверх (от 500 и выше)")]
    [SerializeField] private float _jumpForce;

    private void OnTriggerStay2D(Collider2D collision)
    {
        var _rb2d = collision.gameObject.GetComponent<Rigidbody2D>();
        _rb2d.velocity = Vector2.zero;
        _rb2d.AddForce(new Vector2(0, _jumpForce));
    }

}
