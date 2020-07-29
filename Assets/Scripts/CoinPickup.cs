using Interface;
using UnityEngine;

public class CoinPickup : MonoBehaviour, ICollectable
{
    [SerializeField] private AudioClip _coinPickUpSFX;
    [SerializeField] private int _coinsToAdd = 1;

    private GameSession _gameSession;

    private bool _isCoinPassed;

    private void Start()
    {
        _gameSession = GameSession.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       Collect();
    }

    public void Collect()
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
