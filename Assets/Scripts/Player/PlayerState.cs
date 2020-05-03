using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PolygonCollider2D))]
public class PlayerState : MonoBehaviour
{
    [Tooltip("Когда персонаж умирает, его тело слегка подлетает. Этот вектор отвечает за пуш, который дается телу при смерти")]
    [SerializeField] private Vector2 _deathKick;

    public bool IsAlive = true;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private PolygonCollider2D _bodyCollider;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (IsKilled() && IsAlive)
            Die();
    }

    private bool IsKilled()
    {
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask(Constants.Hazards)))
            return true;
        else
            return false;
    }

    private void Die()
    {
        IsAlive = false;
        _animator.SetTrigger(Constants.Died);
        _rb2d.velocity = Vector2.zero;
        _rb2d.velocity = _deathKick;

        FindObjectOfType<GameSession>().PlayerDeath();
    }
}
