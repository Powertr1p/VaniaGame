﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Collider2D))]
public abstract class MovableObject : MonoBehaviour
{
   [SerializeField] private Transform[] _waypoints;
   [SerializeField] protected float Speed = 10f;
   [SerializeField] private bool _loopBackwards;
   
   protected Transform Target;
   private int _currentWaypoint = 0;
   private bool _isBackwards = false;

   private void Start()
   {
      Init();
   }

   protected virtual void Init()
   {
      Target = _waypoints[_currentWaypoint];
   }

   protected virtual void Update()
   {
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

   private void ChangeTarget()
   {
      ChangeTargetForward();
      ChangeTargetBackward();
   }

   protected virtual void ChangeTargetForward()
   {
      if (_isBackwards) return;
      
      var nextWaypoint = _currentWaypoint + 1;

      if (nextWaypoint < _waypoints.Length) 
            _currentWaypoint = nextWaypoint;
      else if (_loopBackwards)
         _isBackwards = true;
      else
         _currentWaypoint = 0;
      
      Target = _waypoints[_currentWaypoint];
   }

   private void ChangeTargetBackward()
   {
      if (!_isBackwards) return;
      
      var nextWaypoint = _currentWaypoint - 1;

      if (nextWaypoint >= 0)
         _currentWaypoint = nextWaypoint;

      if (nextWaypoint == 0)
         _isBackwards = false;
      
      Target = _waypoints[_currentWaypoint];
   }

   protected void OnCollisionEnter2D(Collision2D other)
   {
      other.collider.transform.SetParent(transform);
   }

   protected void OnCollisionExit2D(Collision2D other)
   {
      other.collider.transform.SetParent(null);
   }
}
