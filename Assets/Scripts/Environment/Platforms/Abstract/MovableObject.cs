using System;
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
   
   protected Transform Target;
   private int _currentWaypoint = 0;

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

   protected virtual void ChangeTarget()
   {
      var nextWaypoint = _currentWaypoint + 1;

      if (nextWaypoint < _waypoints.Length)
         _currentWaypoint = nextWaypoint;
      else
         _currentWaypoint = 0;

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
