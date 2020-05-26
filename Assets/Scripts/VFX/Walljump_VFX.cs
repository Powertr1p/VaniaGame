using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walljump_VFX : MonoBehaviour
{
   public float TimeToDestroy = 0.5f;

   private void Start()
   {
      Destroy(gameObject,TimeToDestroy);
   }
}
