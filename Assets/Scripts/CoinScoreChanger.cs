using UnityEngine;
using UnityEngine.UI;

public class CoinScoreChanger : MonoBehaviour
{
    private GameSession _gameSession;
    private Text _coinScoreText;

    private void Awake()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _coinScoreText = GetComponent<Text>();
    }

    private void Update()
    {
        _coinScoreText.text = _gameSession.CoinsCount.ToString();
    }

}
