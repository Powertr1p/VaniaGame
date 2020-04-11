using UnityEngine;

public abstract class JumpPad : MonoBehaviour, IInteractable
{
    [SerializeField] protected float JumpForce;
    protected Rigidbody2D PlayerRigidBody;

    public abstract void Interact();

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        Interact();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerRigidBody = null;
    }
}
