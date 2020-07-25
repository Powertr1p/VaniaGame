using DG.Tweening;
using UnityEngine;

public class InterpolatedPlatform : MovableObject
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;
    [SerializeField] private bool _loopBackwards;
    
    protected override void Init()
    {
        Waypoints = _waypoints;
        Speed = _speed;
        LoopBackwards = _loopBackwards;
        
        base.Init();
    }

    protected override void Move()
    {
        transform.DOMove(Target.position, _speed);
    }
}
