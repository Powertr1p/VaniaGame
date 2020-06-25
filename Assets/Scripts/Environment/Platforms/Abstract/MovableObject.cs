using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour
{
   [SerializeField] protected Transform Waypoint_A;
   [SerializeField] protected Transform Waypoint_B;
   [SerializeField] protected float Duration = 5f;

   protected Transform Target;
   
   private void Start()
   {
      Target = Waypoint_A;
   }

   protected virtual void Update()
   {
      TryChangeTarget();
      Move();
   }

   protected void Move()
   {
      transform.DOMove(Target.position, Duration);
   }

   protected void TryChangeTarget()
   {
      if (Vector2.Distance(transform.position, Target.position) < 0.5f)
         ChangeTarget();
   }

   protected void ChangeTarget()
   {
      Target = Target.position == Waypoint_A.position ? Waypoint_B : Waypoint_A;
   }
}
