using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private GameObject _wallSlideVFX;

    private PlayerMovement _movement;
    
    private Vector3 _wallFVFXOffset = new Vector3(0.5f, 0f, 0f);
    private int _wallSildeVFXcount = 0;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _movement.OnWallslide += TrySpawnVFX;
    }

    public void TrySpawnVFX()
    {
        if (_wallSildeVFXcount > 0) return;
        
        StartCoroutine(SpawnWallVFX());
    }
    
    private IEnumerator SpawnWallVFX()
    {
        _wallSildeVFXcount++;
        var vfx = Instantiate(_wallSlideVFX, transform.position + _wallFVFXOffset * transform.localScale.x, Quaternion.identity);
        vfx.transform.localScale = transform.localScale;
        
        if (vfx.TryGetComponent(out Wallslide_VFX walljump_VFX))
            yield return new WaitForSeconds(walljump_VFX.TimeToDestroy);

        _wallSildeVFXcount--;
    }
}
