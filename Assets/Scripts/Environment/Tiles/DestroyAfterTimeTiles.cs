using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        TryDestroyBottomTile(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        TryDestroyBottomTile(other);
    }

    private void TryDestroyBottomTile(Collision2D player)
    {
        if (_isSuspended) return;
        
        var tilePosition = GetBottomTile(player.transform.position);
        StartCoroutine(WaitAndDestroyTile(tilePosition));
    }

    private Vector3Int GetBottomTile(Vector3 playerPosition)
    {
        return _tilemap.WorldToCell(playerPosition - new Vector3(0f, 1f));
    }

    private IEnumerator WaitAndDestroyTile(Vector3 tilePosition)
    {
        _isSuspended = true;
        var tilePositionInCell = _tilemap.WorldToCell(GetBottomTile(tilePosition);
        yield return new WaitForSeconds(_destroyTime);
        _tilemap.SetTile(tilePositionInCell, null);
        _isSuspended = false;
    }
}
