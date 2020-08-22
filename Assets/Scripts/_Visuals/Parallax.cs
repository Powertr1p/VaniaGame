using System;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _parallaxMovement = 0.1f;

    private float _cameraOffset;
    private Vector3 _lastPosition;

    private void Start()
    {
        _lastPosition = _camera.transform.position;
    }

    private void LateUpdate()
    {
        float deltaMovement = _camera.transform.position.x - _lastPosition.x;
        transform.position += new Vector3(deltaMovement * _parallaxMovement, 0, 0);
        _lastPosition = _camera.transform.position;
    }
}
