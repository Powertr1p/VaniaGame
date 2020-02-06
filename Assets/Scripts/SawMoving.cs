using UnityEngine;

public class SawMoving : MonoBehaviour
{
    [Header("Start and end position of saw")]
    [Tooltip("Move your saw to final left postion and copy X here")]
    [SerializeField] private Vector3 _leftBorder;
    [Tooltip("Move your saw to final right postion and copy X here")]
    [SerializeField] private Vector3 _rightBorder;

    private Vector3 _currentValue;
    private int _direction = 1;
    private float _speed = 0.5f;

    private void Start()
    {
        _currentValue = transform.localPosition;
    }

    private void Update()
    {
        _currentValue.x += _speed * Time.deltaTime * _direction;
        if (_currentValue.x >= _rightBorder.x || _currentValue.x <= _leftBorder.x)
            _direction = -_direction;

        transform.localPosition = _currentValue;
    }
}


