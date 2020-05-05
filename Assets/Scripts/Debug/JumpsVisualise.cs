using UnityEngine;
using TMPro;

public class JumpsVisualise : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private PlayerMovement _player;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerMovement>();
    }
    

    private void Update()
    {
        ChangeColor();
        _text.text = _player.AmountOfJumps.ToString();
    }

    private void ChangeColor()
    {
        switch (_player.AmountOfJumps)
        {
            case 0: _text.color = Color.red;
                break;
            case 1: _text.color = Color.magenta;
                break;
            case 2: _text.color = Color.green;
                break;
            default: _text.color = Color.cyan;
                break;
        }
    }
    
}
