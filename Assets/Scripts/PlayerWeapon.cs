using System.Collections;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed = 10F;
    private bool _isShooting = false;

    private void OnEnable()
    {
        GetComponentInParent<Player>().OnAttack += Attack;
    }

    private void Attack()
    {
        StartCoroutine(SpawnProjectile());
    }

    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(0.3F);
        GameObject arrow = Instantiate(_projectile, transform.position, Quaternion.identity) as GameObject;
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(transform.localScale.x) * _projectileSpeed, 0);
        yield return new WaitForSeconds(0.4F);
    }

    private void OnDisable()
    {
        if (GetComponentInParent<Player>() != null)
            GetComponentInParent<Player>().OnAttack -= Attack;
    }

}
