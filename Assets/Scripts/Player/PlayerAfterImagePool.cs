using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField] private GameObject _afterImagePrefab;
    
    private Queue<GameObject> _availableObjects = new Queue<GameObject>();
    
    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        GrowPool();
    }
    
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        _availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if (_availableObjects.Count == 0)
            GrowPool();

        var instance = _availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
    
    private void GrowPool()
    {
        for (int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(_afterImagePrefab, transform, true);
            AddToPool(instanceToAdd);
        }
    }
}
