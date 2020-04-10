using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class JumpPad : MonoBehaviour, IInteractable
{
    [SerializeField] protected float JumpForce;
   
    private Rigidbody2D _playerRigidBody;

    public virtual void Interact()
    {
        if (_playerRigidBody == null) return;

        _playerRigidBody.velocity = Vector2.zero;
        _playerRigidBody.AddForce(new Vector2(0, JumpForce));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _playerRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        Interact();
    }

    private void OnTriggerExit(Collider other)
    {
        _playerRigidBody = null;
    }
}
