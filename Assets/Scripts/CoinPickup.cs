using UnityEngine;
using UnityEngine.Events;

public class CoinPickup : MonoBehaviour
{
    public UnityAction OnCoinPickup;

    [SerializeField] private AudioClip _coinPickUpSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            AudioSource.PlayClipAtPoint(_coinPickUpSFX, Camera.main.transform.position);
            OnCoinPickup?.Invoke();
            Destroy(gameObject);
        }
    }
}
