using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HiddenSpikes : MonoBehaviour, ITriggerable
{
    private Animator _animator;

    private const string ActivateAnimation = "Activate";
    private const string DeactivateAnimation = "Deactivate";
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        _animator.SetTrigger(ActivateAnimation);
    }

    public void Deactivate()
    {
        _animator.SetTrigger(DeactivateAnimation);
    }
}
