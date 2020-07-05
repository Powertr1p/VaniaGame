using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyAfterTimeTiles : MonoBehaviour
{
    [Tooltip("Time that must passed before destroy tile. Player broke all block around him (front, back and bottom) at same time")]
    [SerializeField] private float _destroyTime = 0.1f;
    
    private Tilemap _tilemap;
    private Vector3 _playerPosition;
    private bool _isSuspended = false;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _playerPosition = other.transform.position;
        TryDestroyBottomTile();
    }

    private void TryDestroyBottomTile()
    {
        if (_isSuspended) return;
        
        var tilePosition = FindCollidedTile();

        StartCoroutine(WaitAndDestroyTile(tilePosition));
    }

    private Vector3Int FindCollidedTile()
    {
        if (_tilemap.HasTile(GetRightTile()))
            return GetRightTile();
        else if (_tilemap.HasTile(GetLeftTile()))
            return GetLeftTile();
        else if (_tilemap.HasTile(GetBottomTile()))
            return GetBottomTile();
        else if (_tilemap.HasTile(GetLeftBottomTile()))
            return GetLeftBottomTile();
        else 
           return GetRightBottomTile();
    }

    private Vector3Int GetRightBottomTile()
    {
        return _tilemap.WorldToCell(_playerPosition + new Vector3(1, 1, 0));
    }
    
    private Vector3Int GetLeftBottomTile()
    {
        return _tilemap.WorldToCell(_playerPosition + new Vector3(-1, -1, 0));
    }
    
    private Vector3Int GetRightTile()
    {
        return _tilemap.WorldToCell(_playerPosition + Vector3.right);
    }

    private Vector3Int GetLeftTile()
    {
        return _tilemap.WorldToCell(_playerPosition + Vector3.left);
    }

    private Vector3Int GetBottomTile()
    {
        return _tilemap.WorldToCell(_playerPosition + Vector3.down);
    }

    private IEnumerator WaitAndDestroyTile(Vector3Int tilePosition)
    {
        _isSuspended = true;
        yield return new WaitForSeconds(_destroyTime);
        _tilemap.SetTile(tilePosition, null);
        _isSuspended = false;
    }
}
