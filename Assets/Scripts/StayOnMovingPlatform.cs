using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StayOnMovingPlatform : MonoBehaviour
{
    protected void OnCollisionEnter2D(Collision2D other)
    {
        other.collider.transform.SetParent(transform);
    }

    protected void OnCollisionExit2D(Collision2D other)
    {
        other.collider.transform.SetParent(null);
    }
}
