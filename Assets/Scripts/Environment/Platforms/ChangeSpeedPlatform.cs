using Environment.Platforms;
using UnityEngine;

public class ChangeSpeedPlatform : MovableObject
{
    [SerializeField] private Waypoint _firstWaypoint;
    private Waypoint _currentWaypoint;

    protected override void Init()
    {
        _currentWaypoint = _firstWaypoint;
        Speed = _firstWaypoint.GetSpeed();
        Target = _currentWaypoint.transform;

        StartCoroutine(WaitBeforeStartMoving());
    }

    protected override void ChangeTarget()
    {
        var nextWaypoint = _currentWaypoint.GetNextWaypoint();
        Speed = _currentWaypoint.GetSpeed();
        _currentWaypoint = nextWaypoint != null ? nextWaypoint : _firstWaypoint;
        Target = _currentWaypoint.transform;
    }
}
