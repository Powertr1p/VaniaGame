using System;
using UnityEngine;
using TMPro;

public class VelocityVisualise : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textX;
    [SerializeField] private TextMeshProUGUI _textY;
    private PlayerMovement _player;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerMovement>();
    }
        
    private void Update()
    {
        _textX.text = CropToTwoDigitsAfterComma(_player.CurrentPlayerVelocity.x).ToString();
        _textY.text = CropToTwoDigitsAfterComma(_player.CurrentPlayerVelocity.y).ToString();
    }

    private double CropToTwoDigitsAfterComma(float digits)
    {
        double doubled = digits;
        var croppedDigits = Math.Round(doubled, 2);

        return croppedDigits;
    }
}
