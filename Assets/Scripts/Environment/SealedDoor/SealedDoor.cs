using System;
using System.Collections.Generic;
using Environment.SealedDoor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SealedDoor : MonoBehaviour
{
    [SerializeField] private List<Key> _keys;

    private void OnEnable()
    {
        foreach (var key in _keys)
        {
            key.OnPickedUp += RemovePickedKey;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_keys.Count == 0)
            OpenDoor();
    }

    private void OpenDoor()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void RemovePickedKey(Key key)
    {
        key.OnPickedUp -= RemovePickedKey;
        _keys.Remove(key);
    }

    #region DEV

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    
    #endregion
}
