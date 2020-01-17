using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private HorizontalScroll _movingWall;

    private void Start()
    {
        _movingWall = GetComponentInParent<HorizontalScroll>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            _movingWall.IsStarted = true;
            _movingWall.DestroyItself();
        }
    }

}
