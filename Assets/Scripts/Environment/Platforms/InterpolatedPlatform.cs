using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InterpolatedPlatform : MovableObject
{
    protected override void Move()
    {
        transform.DOMove(Target.position, Speed);
    }
}
