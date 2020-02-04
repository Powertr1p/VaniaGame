using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed = 10F;

    private Animator _animator;

    private const string _shootingAnimation = "Shoot";
    public bool IsShooting { get; private set; }

    private void OnEnable()
    {
        GetComponentInParent<PlayerInput>().OnAttack += Attack;
    }

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }

    private void Attack()
    {
        if (!IsShooting && !GetComponentInParent<PlayerInput>().IsClimbing)
        {
            _animator.SetTrigger(_shootingAnimation);
            StartCoroutine(SpawnProjectile());
        }
    }

    private IEnumerator SpawnProjectile()
    {
        IsShooting = true;
        yield return new WaitForSeconds(0.3F);
        GameObject arrow = Instantiate(_projectile, transform.position, Quaternion.identity) as GameObject;
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _projectileSpeed, 0);
        yield return new WaitForSeconds(0.4F);
        IsShooting = false;
    }

    private void OnDisable()
    {
        if (GetComponentInParent<PlayerInput>() != null)
            GetComponentInParent<PlayerInput>().OnAttack -= Attack;
    }
}
