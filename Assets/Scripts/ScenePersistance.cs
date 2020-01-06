using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistance : MonoBehaviour
{
    private int _startingSceneIndex;

    private void Awake()
    {
        if (FindObjectsOfType<ScenePersistance>().Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != _startingSceneIndex)
            Destroy(gameObject);
    }

}
