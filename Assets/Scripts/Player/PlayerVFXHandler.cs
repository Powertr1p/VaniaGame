using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerVFXHandler : MonoBehaviour
{
    [Header("Wallslide VFX Config")]
    [SerializeField] private GameObject _wallSlideVFX;
    [SerializeField] private int _maxWallslideVFXOnSсreen = 0;

    [Header("Falling VFX")] 
    [SerializeField] private float _distanceBetweenImagesFalling = 0.2f;
    [SerializeField] private float _fallingSpeedToActivateVFX = -40;
    private float _lastImageYPos;

    [Header("Dash VFX")] 
    [SerializeField] private float _distanceBetweenImagesDashing = 0.1f;
    private float _lastImageXPos;
    
    private PlayerMovement _movement;
    
    private Vector3 _wallFVFXOffset = new Vector3(0.4f, 0f, 0f);
    private int _currentWallSildeVFXcount = 0;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _movement.OnWallslide += TrySpawnWallslideVFX;
        _movement.OnDashing += TrySpawnAfterImageWhileDashing;
        _movement.OnAttemptToDash += SpawnAfterImageWhileDashing;
    }

    private void Update()
    {
        if (_movement.CurrentPlayerVelocity.y <= _fallingSpeedToActivateVFX)
            SpawnAfterImageWhileFalling();
    }

    private void TrySpawnWallslideVFX()
    {
        if (_currentWallSildeVFXcount > _maxWallslideVFXOnSсreen) return;
        
        StartCoroutine(SpawnWallslideVFX());
    }
    
    private IEnumerator SpawnWallslideVFX()
    {
        _currentWallSildeVFXcount++;
        
        var vfx = Instantiate(_wallSlideVFX, transform.position + _wallFVFXOffset * transform.localScale.x, Quaternion.identity);
        vfx.transform.localScale = transform.localScale;
        
        if (vfx.TryGetComponent(out Wallslide_VFX wallslideVFX))
            yield return new WaitForSeconds(wallslideVFX.TimeToDestroy);

        _currentWallSildeVFXcount--;
    }

    private void SpawnAfterImageWhileFalling()
    {
        if (Mathf.Abs(transform.position.y - _lastImageYPos) > _distanceBetweenImagesFalling)
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            _lastImageYPos = transform.position.y;
        }
    }

    private void TrySpawnAfterImageWhileDashing()
    {
        if (Mathf.Abs(transform.position.x - _lastImageXPos) > _distanceBetweenImagesDashing)
        {
            SpawnAfterImageWhileDashing();
        }
    }

    private void SpawnAfterImageWhileDashing()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        _lastImageXPos = transform.position.x;
    }


    private void OnDisable()
    {
        _movement.OnWallslide -= TrySpawnWallslideVFX;
        _movement.OnDashing -= TrySpawnAfterImageWhileDashing;
        _movement.OnAttemptToDash -= SpawnAfterImageWhileDashing;
    }
}
