using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _parallaxMovement = 1f;

    private float _cameraOffset;
    
    
    private void Update()
    {
        transform.position = new Vector2(_camera.position.x  * _parallaxMovement , transform.position.y);
    }
}
