using System;
using UnityEngine;

namespace Environment.Tiles
{
    [RequireComponent(typeof(Collider2D))]
    public class DashRechargeTiles : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                player.RechargeDash();
            }
        }
    }
}