using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    public UnityAction OnLevelPassed;

    [SerializeField] private float _levelLoadDelay = 0.1F;
    private int _nextScene;

    private void Start()
    {
        _nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerState>())
            StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(_levelLoadDelay);
        OnLevelPassed?.Invoke();
        SceneManager.LoadScene(_nextScene);
    }
}
