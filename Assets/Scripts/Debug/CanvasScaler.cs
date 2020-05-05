using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScaler : MonoBehaviour
{
    private PlayerMovement _player;
    private Transform _transform;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerMovement>();
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        _transform.localScale = _player.transform.localScale;
    }
}
