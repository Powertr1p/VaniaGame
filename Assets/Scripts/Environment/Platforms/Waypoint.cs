using System.Collections;
using UnityEngine;

namespace Environment.Platforms
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private Waypoint _nextWaypoint;
        [SerializeField] private float _speedBetweenCurrentAndNext;
        
        public Waypoint GetNextWaypoint()
        {
            return _nextWaypoint != null ? _nextWaypoint : null;
        }

        public float GetSpeed()
        {
            return _speedBetweenCurrentAndNext;
        }
    }
}