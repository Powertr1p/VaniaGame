using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalScroll : MonoBehaviour
{
    [Tooltip("Game units per second")]
    [SerializeField] private float _scrollRate = 1F;

    private bool _isStarted = false;

private void Update()
{
    while(_isStarted)
    {
        transform.Translate(new Vector2(_scrollRate * Time.deltaTime, 0f));
    }
}

private void OnTriggerEnter2D (Collider2D collision)
{
    _isStarted = true;
}
    
    private void DestroyItself() => Destroy(gameObject, 20f);

}
