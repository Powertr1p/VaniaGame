using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public int PlayerLives { get; private set; }

    private void Awake()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        PlayerLives = 3;
    }

    public void ProcessPlayerDeath()
    {
        if (PlayerLives > 0)
            StartCoroutine(TakeLife());
        else
            StartCoroutine(ResetGameSession());
    }

    private IEnumerator TakeLife()
    {
        PlayerLives--;
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
