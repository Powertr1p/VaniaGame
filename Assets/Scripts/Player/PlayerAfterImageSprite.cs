using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] private float _activeTime = 0.1f;
    [SerializeField] float _alphaSet = 0.8f;
    [SerializeField] private float _alphaMultiplayer = 0.85f;
    private float _timeActivated;
    private float _alpha;
    
    private SpriteRenderer _playerSprite;
    private Transform _player;
    private SpriteRenderer _sprite;

    private Color _color;

    private void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _player = FindObjectOfType<PlayerMovement>().transform;
        _playerSprite = _player.GetComponent<SpriteRenderer>();

        _alpha = _alphaSet;
        _sprite.sprite = _playerSprite.sprite;
        transform.position = _player.position;
        transform.rotation = _player.rotation;
        _timeActivated = Time.time;
    }

    private void Update()
    {
        _alpha *= _alphaMultiplayer;
        _color = new Color(1f,1f,1f,_alpha);
        _sprite.color = _color;

        if (Time.time >= (_timeActivated + _activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);   
        }
    }
}
