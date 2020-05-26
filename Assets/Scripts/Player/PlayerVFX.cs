using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private GameObject _wallSlideVFX;
    
    public void SpawnVFX() 
    {
        var vfx = Instantiate(_wallSlideVFX, transform.position, Quaternion.identity);
            vfx.transform.localScale = transform.localScale;
    }
}
