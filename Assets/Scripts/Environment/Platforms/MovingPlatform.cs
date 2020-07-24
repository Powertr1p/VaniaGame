using UnityEngine;

namespace Environment.Platforms
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Waypoint _firstWaypoint;

        private Transform _target => _currentWaypoint.transform;
        private Waypoint _currentWaypoint;
        private float _speed;

        private void Start()
        {
            _currentWaypoint = _firstWaypoint;
            _speed = _firstWaypoint.GetSpeed();
        }
        
        private void Update()
        {
            Move();
            TryChangeTarget();
        }
        
        private void Move()
        {
            transform.position =  Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        }

        private void TryChangeTarget()
        {
            if (Vector2.Distance(transform.position, _target.position) < 0.5f)
                ChangeTarget();
        }

        private void ChangeTarget()
        {
            var nextWaypoint = _currentWaypoint.GetNextWaypoint();
            _speed = _currentWaypoint.GetSpeed();
            _currentWaypoint = nextWaypoint != null ? nextWaypoint : _firstWaypoint;
        }
    }
}