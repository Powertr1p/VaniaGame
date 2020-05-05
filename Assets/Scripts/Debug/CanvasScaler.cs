using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScaler : MonoBehaviour
{
    private PlayerMovement _player;
    private Canvas _canvas;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerMovement>();
        _canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        UnityEngine.Debug.Log(_player.transform.localScale.x);
        
        if (_player.transform.localScale.x == -1f)
        {
            _canvas.transform.localScale = new Vector3(-1f,1f,1f);
        }
        else
        {
            _canvas.transform.localScale = new Vector3(1f,1f,1f);
        }
    }
}
