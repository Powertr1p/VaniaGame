using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerForObject : MonoBehaviour
{
    [SerializeField] private GameObject _objectToTrigger;
    [SerializeField] private float _delayBeforeTrigger = 0f;
    
    private ITriggerable _trigger;

    private void Start()
    {
        _trigger = _objectToTrigger.GetComponent<ITriggerable>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
            StartCoroutine(WaitAndToggle(_trigger.Activate));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
            StartCoroutine(WaitAndToggle(_trigger.Deactivate));
    }

    private IEnumerator WaitAndToggle(Action callback)
    {
        yield return new WaitForSeconds(_delayBeforeTrigger);
        callback?.Invoke();
    }
}
