using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesWithTimer : Spikes
{
   [SerializeField] private float _delayBeforeActivation = 2f;
   [SerializeField] private float _delayBeforeDeactivation = 1f;
   [SerializeField] private float _firstActivationDelay = 6f;
   [SerializeField] private bool _deactivatedAtStart = true;
   
   private const string Activate = "Activate";
   private const string Deactivate = "Deactivate";
   
   protected override void Init()
   {
      base.Init();

      if (_deactivatedAtStart)
         Deactivate();
      
      StartCoroutine(WaitAndToggle());
   }

   private IEnumerator WaitAndToggle()
   {
      if (_firstActivationDelay > 0)
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
