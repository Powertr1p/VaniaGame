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
        if (collision.gameObject.GetComponent<PlayerState>())
        {
            _movingWall.IsStarted = true;
            _movingWall.DestroyItself();
        }
    }

}
