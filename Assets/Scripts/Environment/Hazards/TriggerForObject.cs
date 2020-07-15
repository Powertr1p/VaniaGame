using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerForObject : MonoBehaviour
{
    [SerializeField] private GameObject _objectToTrigger;
    [SerializeField] private float _delayBeforeActivation = 0f;
    [SerializeField] private float _delayBeforeDeactivation = 0f;
    [SerializeField] private bool _reverseBehavior;
    private bool _isSuspended;
    
    private ITriggerable _trigger;

    private const string Reverse = "Reverse";

    private void Start()
    {
        _trigger = _objectToTrigger.GetComponent<ITriggerable>();
        _objectToTrigger.GetComponent<Animator>().SetBool(Reverse, _reverseBehavior);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isSuspended) return;
        
        if (other.gameObject.GetComponent<PlayerMovement>() == null) return;

        StartCoroutine(WaitAndToggle());
    }

    private IEnumerator WaitAndToggle()
    {
        _isSuspended = true;
        
        if (_reverseBehavior)
        {
            yield return new WaitForSeconds(_delayBeforeDeactivation);
            _trigger.Deactivate();

            yield return new WaitForSeconds(_delayBeforeActivation);
            _trigger.Activate();
        }
        else
        {
            yield return new WaitForSeconds(_delayBeforeActivation);
            _trigger.Activate();
            
            yield return new WaitForSeconds(_delayBeforeDeactivation);
            _trigger.Deactivate();
        }

        _isSuspended = false;
    }
}
