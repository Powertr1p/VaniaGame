using UnityEngine;

public class SawMoving : MonoBehaviour
{
    [Header("Start and end position of saw")]
    [Tooltip("Move your saw to final left postion and copy X here")]
    [SerializeField] private Vector3 _leftBorder;
    [Tooltip("Move your saw to final right postion and copy X here")]
    [SerializeField] private Vector3 _rightBorder;
    [SerializeField] private float _speed = 0.4f;

    private Vector3 _currentValue;
    private int _direction = 1;

    private void Start()
    {
        _currentValue = transform.localPosition;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _currentValue.x += _speed * Time.deltaTime * _direction;
        TryChangeDirection();

        transform.localPosition = _currentValue;
    }

    private void TryChangeDirection()
    {
        if (_currentValue.x >= _rightBorder.x || _currentValue.x <= _leftBorder.x)
            _direction = -_direction;
    }
}


