using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyAfterTimeTiles : MonoBehaviour
{
    [Tooltip("Time that must passed before destroy tile. Player brokes all block around him (front, back and bottom) at same time")]
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
        
        var tilePosition = GetBottomTile();
        StartCoroutine(WaitAndDestroyTile(tilePosition));
    }

    private Vector3Int GetBottomTile()
    {
        return _tilemap.WorldToCell(_playerPosition - new Vector3(0f, 1f));
    }

    private IEnumerator WaitAndDestroyTile(Vector3Int tilePosition)
    {
        _isSuspended = true;
        yield return new WaitForSeconds(_destroyTime);
        _tilemap.SetTile(tilePosition, null);
        _isSuspended = false;
    }
}
