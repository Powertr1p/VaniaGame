using UnityEngine;

public class DashRecharge : MonoBehaviour
{
    private const float delayBeforeDestroy = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
            StartCoroutine(player.RechargeDash());

        Destroy(gameObject, delayBeforeDestroy);
        Destroy(gameObject.GetComponent<SpriteRenderer>());
        Destroy(gameObject.GetComponent<CircleCollider2D>());
    }
}
