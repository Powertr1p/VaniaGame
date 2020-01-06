using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickUpSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            AudioSource.PlayClipAtPoint(_coinPickUpSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
