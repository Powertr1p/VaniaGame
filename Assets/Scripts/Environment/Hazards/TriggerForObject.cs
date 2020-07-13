using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerForObject : MonoBehaviour
{
    [SerializeField] private GameObject _objectToTrigger;
    [SerializeField] private float _delayBeforeOpen = 0f;
    [SerializeField] private float _delayBeforeClose = 0f;
    [SerializeField] private bool _reverseBehavior;
    
    private ITriggerable _trigger;
    

    private void Start()
    {
        _trigger = _objectToTrigger.GetComponent<ITriggerable>();
        _objectToTrigger.GetComponent<Animator>().SetBool("Reverse", _reverseBehavior);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() == null) return;

        StartCoroutine(!_reverseBehavior ? 
            WaitAndToggle(_trigger.Activate, _delayBeforeOpen) : WaitAndToggle(_trigger.Deactivate, _delayBeforeClose));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StartCoroutine(!_reverseBehavior ? 
            WaitAndToggle(_trigger.Deactivate, _delayBeforeClose) : WaitAndToggle(_trigger.Activate, _delayBeforeOpen));
    }

    private IEnumerator WaitAndToggle(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
