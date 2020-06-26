using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesWithTimer : Spikes
{
   [SerializeField] private float _delayBeforeActivation = 2f;
   [SerializeField] private float _delayBeforeDeactivation = 1f;
   
   protected override void Init()
   {
      Animator = GetComponent<Animator>();

      StartCoroutine(LoopActivation());
   }

   private IEnumerator LoopActivation()
   {
      while (true)
      {
         yield return new WaitForSeconds(_delayBeforeActivation);
         Activate();
        
         yield return new WaitForSeconds(_delayBeforeDeactivation);
         Deactivate();
      }
   }
}
