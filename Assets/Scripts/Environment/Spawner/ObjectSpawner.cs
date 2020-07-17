using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour, ITriggerable
{
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _destination;
    
    [SerializeField] private bool _isUsedByTrigger = false;
    [SerializeField] private float _delayBeforeFirstStart = 0f;
    [SerializeField] private float _delayBeforeActivation = 1f;
    
    private void Start()
    {
        if (!_isUsedByTrigger)
            InvokeRepeating("Activate", _delayBeforeFirstStart, _delayBeforeActivation); 
    }

    public void Activate()
    {
        var spawnedObject = Instantiate(_objectToSpawn,transform.position, Quaternion.identity);
        spawnedObject.GetComponent<SpawnableObject>().Init(_destination, _speed);
        spawnedObject.transform.SetParent(transform);
    }

    public void Deactivate()
    {
    }
}
