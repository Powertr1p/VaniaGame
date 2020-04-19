using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerState _player;
    [SerializeField] private MianBossDissapears _bossScript;
    [SerializeField] private Canvas _gameCanvas;

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
        _bossScript.MakeLaugh();
        Cursor.visible = false;
        _gameCanvas.gameObject.SetActive(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(0);        
    }
}
