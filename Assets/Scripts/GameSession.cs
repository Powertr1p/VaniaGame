using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int _playerLives = 3;

    private void Awake()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    public void ProcessPlayerDeath()
    {
        if (_playerLives > 1)
            StartCoroutine(TakeLife());
        else
            StartCoroutine(ResetGameSession());
    }

    private IEnumerator TakeLife()
    {
        _playerLives--;
        yield return new WaitForSecondsRealtime(2F);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(2F);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
