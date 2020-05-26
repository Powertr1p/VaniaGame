using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour //придумать более явное название
{
    [SerializeField] private GameObject _wallSlideVFX;

    [SerializeField] private int _maxWallslideVFXOnSсreen = 0;

    private PlayerMovement _movement;
    
    private Vector3 _wallFVFXOffset = new Vector3(0.4f, 0f, 0f);
    private int _wallSildeVFXcount = 0;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _movement.OnWallslide += TrySpawnWallslideVFX;
    }

    private void TrySpawnWallslideVFX()
    {
        if (_wallSildeVFXcount > _maxWallslideVFXOnSсreen) return;
        
        StartCoroutine(SpawnWallslideVFX());
    }
    
    private IEnumerator SpawnWallslideVFX()
    {
        _wallSildeVFXcount++;
        
        var vfx = Instantiate(_wallSlideVFX, transform.position + _wallFVFXOffset * transform.localScale.x, Quaternion.identity);
        vfx.transform.localScale = transform.localScale;
        
        if (vfx.TryGetComponent(out Wallslide_VFX wallslideVFX))
            yield return new WaitForSeconds(wallslideVFX.TimeToDestroy);

        _wallSildeVFXcount--;
    }

    private void OnDisable()
    {
        _movement.OnWallslide -= TrySpawnWallslideVFX;
    }
}
