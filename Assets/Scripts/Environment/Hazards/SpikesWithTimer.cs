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
      base.Init();
      
      StartCoroutine(WaitAndToggle());
   }

   private IEnumerator WaitAndToggle()
   {
      yield return new WaitForSeconds(_firstActivationDelay);
      
      while (true)
      {
         yield return new WaitForSeconds(_delayBeforeActivation);
         Activate();
         
         yield return new WaitForSeconds(_delayBeforeDeactivation);
         Deactivate();
      }
   }
}
