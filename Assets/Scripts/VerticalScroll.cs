using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [Tooltip("Game units per second")]
    [SerializeField] private float _scrollRate = 0.5F;

    private void Update()
    {
        transform.Translate(new Vector2(0f,_scrollRate * Time.deltaTime));
    }
}
