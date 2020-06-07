using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyAfterTimeTiles : MonoBehaviour
{
    [Tooltip("Time that must passed before destroy tile")] [SerializeField]
    private float _destroyTime = 0.1f;
    
    private Tilemap _tilemap;
    
    private bool _isSuspended = false;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_isSuspended) return;
        
        StartCoroutine(WaitAndDestroy(other));
    }

    private IEnumerator WaitAndDestroy(Collision2D other)
    {
        _isSuspended = true;
        var playerPosition = _tilemap.WorldToCell(other.transform.position - new Vector3(0f, 1f));
        yield return new WaitForSeconds(_destroyTime);
        _tilemap.SetTile(playerPosition, null);
        _isSuspended = false;
    }
}
