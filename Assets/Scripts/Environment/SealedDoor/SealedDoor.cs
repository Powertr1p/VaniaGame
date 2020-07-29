using System.Collections.Generic;
using System.Linq;
using Environment.SealedDoor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SealedDoor : MonoBehaviour
{
    [SerializeField] private List<Key> _keys;
    [SerializeField] private List<SpriteRenderer> _placeholders;

    private void OnEnable()
    {
        foreach (var key in _keys)
            key.OnPickedUp += RemovePickedKey;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_keys.Count == 0)
            OpenDoor();
    }

    private void OpenDoor()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        HidePlaceholders();
    }

    private void HidePlaceholders()
    {
        foreach (var placeholder in _placeholders)
            placeholder.color = Color.clear;
    }

    private void RemovePickedKey(Key key)
    {
        PlaceKeySpriteOnDoor(key.GetComponent<SpriteRenderer>());
        
        key.OnPickedUp -= RemovePickedKey;
        _keys.Remove(key);
    }

    private void PlaceKeySpriteOnDoor(SpriteRenderer keySprite)
    {
        var placeholder =  _placeholders.FirstOrDefault(place => place.sprite == null);
        if (placeholder != null)
        {
            placeholder.sprite = keySprite.sprite;
            placeholder.color = keySprite.color;
        }
    }

    #region DEV

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    
    #endregion
}
