using UnityEngine;

public class SimpleJumpPad : JumpPad
{
    public override void Interact()
    {
        PlayerRigidBody.velocity = Vector2.zero;
        PlayerRigidBody.AddForce(new Vector2(0, JumpForce));
    }
}
