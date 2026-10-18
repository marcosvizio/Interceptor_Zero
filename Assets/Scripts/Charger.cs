/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-06 23:54:38
 Last Modification Date: 2026-10-18 20:04:52

*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Charger : Enemy
{

    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _contactDamage = 20f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _turnSpeed = 180f;

    private float _nextAttackTime;
    private Rigidbody _rb;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }
    
    protected override void Behave()
    {
        if (!IsPlayerInRange())
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
            return;
        }
        
        Vector3 direction = _player.position - transform.position;

        direction.y = 0f;

        direction = direction.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRotation, _turnSpeed * Time.deltaTime));

        _rb.linearVelocity = new Vector3(direction.x * _moveSpeed, _rb.linearVelocity.y, direction.z * _moveSpeed);
    }

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (_nextAttackTime >= Time.time) return;

        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(_contactDamage);
        }

        _nextAttackTime = Time.time + _attackCooldown;
    }
}
