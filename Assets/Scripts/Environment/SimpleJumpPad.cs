using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SimpleJumpPad : JumpPad
{
    public override void Interact()
    {
        if (PlayerRigidBody == null) return;

        PlayerRigidBody.velocity = Vector2.zero;
        PlayerRigidBody.AddForce(new Vector2(0, JumpForce));
    }
}
