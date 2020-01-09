using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        { 
            _player.IsAlive = false;
            Cursor.visible = true;
        }
    }

    public void StartFirstLevel()
    {
        _player.IsAlive = true;
        FindObjectOfType<MianBossDissapears>().MakeLaugh();
        Cursor.visible = false;
        FindObjectOfType<Canvas>().gameObject.SetActive(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(0);        
    }
}
