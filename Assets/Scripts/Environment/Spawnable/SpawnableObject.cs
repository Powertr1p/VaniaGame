using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    private Transform _destination;
    private float _speed;

    public void Init(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }
    private void FixedUpdate()
    {
        transform.position =  Vector2.MoveTowards(transform.position, _destination.position, _speed * Time.deltaTime);
    }
}
