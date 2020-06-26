using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Spikes : MonoBehaviour, ITriggerable
{
    protected Animator Animator;
    
    protected const string ActivateAnimation = "Activate";
    protected const string DeactivateAnimation = "Deactivate";

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        Animator.SetTrigger(ActivateAnimation);
    }

    public void Deactivate()
    {
        Animator.SetTrigger(DeactivateAnimation);
    }
}
