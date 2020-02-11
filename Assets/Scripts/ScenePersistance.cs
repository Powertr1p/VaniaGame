using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistance : MonoBehaviour
{
    private static ScenePersistance _instance = null;
    private int _startingSceneIndex;

    private void Start()
    {
        if (!_instance)
        {
            _instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
            _startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_startingSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            _instance = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }
}
