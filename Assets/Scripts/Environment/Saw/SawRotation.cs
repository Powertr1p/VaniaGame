using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawRotation : MonoBehaviour
{
    private float _speed = 150.0f;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, _speed * Time.deltaTime, Space.Self);
    }
}
