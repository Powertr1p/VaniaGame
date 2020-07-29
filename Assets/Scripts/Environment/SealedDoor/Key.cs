using System;
using Interface;
using UnityEngine;

namespace Environment.SealedDoor
{
    [RequireComponent(typeof(Collider2D))]
    public class Key : MonoBehaviour, ICollectable
    {
        public event Action<Key> OnPickedUp;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Collect();
        }

        public void Collect()
        {
            OnPickedUp?.Invoke(this);
            Destroy(gameObject);
        }
    }
}