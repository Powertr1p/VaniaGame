﻿using System.Collections;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour, ITriggerable
{
   [SerializeField] private float _delayBeforeStart = 0f;
   
   protected Transform[] Waypoints;
   protected float Speed = 10f;
   protected bool LoopBackwards;
   
   protected Transform Target;
   
   private int _currentPoint = 0;
   private bool _isBackwards = false;
   private bool _isStarted = false;

   private void Start()
   {
      Init();
   }

   protected virtual void Init()
   {
      Target = Waypoints[_currentPoint];
      
      StartCoroutine(WaitBeforeStartMoving());
   }

   protected virtual void Update()
   {
      if (!_isStarted) return;
      
      TryChangeTarget();
      Move();
   }

   protected virtual void Move()
   {
      transform.position =  Vector2.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
   }

   protected virtual void TryChangeTarget()
   {
      if (Vector2.Distance(transform.position, Target.position) < 0.5f)
         ChangeTarget();
   }

   protected virtual void ChangeTarget()
   {
      ChangeTargetForward();
      ChangeTargetBackward();
   }

   protected IEnumerator WaitBeforeStartMoving()
   {
      yield return new WaitForSeconds(_delayBeforeStart);
      _isStarted = true;
   }
   
   private void ChangeTargetForward()
   {
      if (_isBackwards) return;
      
      var nextWaypoint = _currentPoint + 1;

      if (nextWaypoint < Waypoints.Length) 
            _currentPoint = nextWaypoint;
      else if (LoopBackwards)
         _isBackwards = true;
      else
         _currentPoint = 0;
      
      Target = Waypoints[_currentPoint];
   }

   private void ChangeTargetBackward()
   {
      if (!_isBackwards) return;
      
      var nextWaypoint = _currentPoint - 1;

      if (nextWaypoint >= 0)
         _currentPoint = nextWaypoint;

      if (nextWaypoint == 0)
         _isBackwards = false;
      
      Target = Waypoints[_currentPoint];
   }
   
   public void Activate()
   {
      LoopBackwards = true;
   }

   public void Deactivate()
   {
      LoopBackwards = false;
   }
}
