using UnityEngine;

namespace Environment.Platforms
{
    public class MovingPlatform : MovableObject
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
    }
}