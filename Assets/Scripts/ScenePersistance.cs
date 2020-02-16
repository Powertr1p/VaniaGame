using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistance : Singleton
{
    private int _startingSceneIndex;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_startingSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            Instance = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }
}
