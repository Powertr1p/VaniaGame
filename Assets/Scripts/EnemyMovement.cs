using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.2F;
    private Rigidbody2D _rb2d;

    private EnemyGroundChecker _groundChecker;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponentInChildren<EnemyGroundChecker>();
        _groundChecker.OnDirectionChange += MoveOppositeSide;
    }

    private void Update()
    {
        _rb2d.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _speed, _rb2d.velocity.y);
    }

    private void MoveOppositeSide()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_rb2d.velocity.x)), 1F);
    }
}
