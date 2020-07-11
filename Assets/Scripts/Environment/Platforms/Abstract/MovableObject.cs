﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class MovableObject : MonoBehaviour
{
   [SerializeField] protected Transform Waypoint_A;
   [SerializeField] protected Transform Waypoint_B;
   [SerializeField] protected float Speed = 10f;
   
   protected Transform Target;
   
   private void Start()
   {
      Init();
   }

   protected virtual void Init()
   {
      Target = Waypoint_A;
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
      Target = Target.position == Waypoint_A.position ? Waypoint_B : Waypoint_A;
   }
}