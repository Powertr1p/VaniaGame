using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Start()
    {
        _player.IsAlive = false;
    }

    public void StartFirstLevel()
    {
        _player.IsAlive = true;
        gameObject.SetActive(false);
    }
}
