using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRecharge : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {

        }

        Destroy(gameObject);
    }
}
