using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            _player.IsAlive = false;
    }

    public void StartFirstLevel()
    {
        _player.IsAlive = true;
        gameObject.SetActive(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(0);        
    }
}
