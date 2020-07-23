using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesWithTimer : Spikes
{
   [SerializeField] private float _delayBeforeActivation = 2f;
   [SerializeField] private float _delayBeforeDeactivation = 1f;
   [SerializeField] private float _firstActivationDelay = 6f;

   private const string Activate = "Activate";
   private const string Deactivate = "Deactivate";
   
   protected override void Init()
   {
      Animator = GetComponent<Animator>();

      InvokeRepeating(Activate, _firstActivationDelay, _delayBeforeActivation);
      InvokeRepeating(Deactivate, _delayBeforeActivation + _firstActivationDelay, _delayBeforeDeactivation);
   }
}
