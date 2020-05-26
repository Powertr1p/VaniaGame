using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private GameObject _wallSlideVFX;

    private int _wallSildeVFXcount = 0;

    public void TrySpawnVFX()
    {
        if (_wallSildeVFXcount > 0) return;
        StartCoroutine(SpawnVFX());
    }
    
    
    private IEnumerator SpawnVFX()
    {
        _wallSildeVFXcount++;
        var vfx = Instantiate(_wallSlideVFX, transform.position + new Vector3(0.5f * transform.localScale.x,0,0), Quaternion.identity);
        vfx.transform.localScale = transform.localScale;
        
        yield return  new WaitForSeconds(0.5f);
        Destroy(vfx);
        _wallSildeVFXcount--;
    }
}
