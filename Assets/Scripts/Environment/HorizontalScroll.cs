using UnityEngine;

public class HorizontalScroll : MonoBehaviour
{
    [Tooltip("Game units per second")]
    [SerializeField] private float _scrollRate = 1F;

    private bool _isStarted = false;
    public bool IsStarted { get => _isStarted; set => _isStarted = value; }

    private void Update()
    {
        if(IsStarted)
            transform.Translate(new Vector2(_scrollRate * Time.deltaTime, 0f));
    }
    
    public void DestroyItself() => Destroy(gameObject, 20f);
}
