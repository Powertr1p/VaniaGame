using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour, ITriggerable
{
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _destination;

    private bool _isUsedByTrigger = false;
    
    private void Start()
    {
        Activate();
    }

    public void Activate()
    {
        var spawnedObject = Instantiate(_objectToSpawn,transform.position, Quaternion.identity);
        spawnedObject.GetComponent<SpawnableObject>().Init(_destination, _speed);
    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
