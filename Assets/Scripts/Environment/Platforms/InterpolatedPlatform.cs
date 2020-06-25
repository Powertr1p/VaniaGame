using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InterpolatedPlatform : MovableObject
{
    [SerializeField] private float _duration = 5f;
        
    protected override void Move()
    {
        transform.DOMove(Target.position, _duration);
    }
}
