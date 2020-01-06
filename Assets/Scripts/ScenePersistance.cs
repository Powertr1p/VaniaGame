using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistance : MonoBehaviour
{
    private static ScenePersistance _insance = null;
    private int _startingSceneIndex;

    private void Start()
    {
        if (!_insance)
        {
            _insance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            _startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
            DontDestroyOnLoad(gameObject);
        }
        else if (_insance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_startingSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            _insance = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }
}
