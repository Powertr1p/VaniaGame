using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickUpSFX;
    [SerializeField] private GameSession _gameSession;
    [SerializeField] private int _coinsToAdd = 1;

    private bool _isCoinPassed;

    private void Awake()
    {
        _gameSession = FindObjectOfType<GameSession>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isCoinPassed)
        {
            _isCoinPassed = true;
            AudioSource.PlayClipAtPoint(_coinPickUpSFX, Camera.main.transform.position, 0.3F);
            _gameSession.IncreaseCoinsCount(_coinsToAdd);
            Destroy(gameObject);
        }
    }
}
