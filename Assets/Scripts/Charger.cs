/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-06 23:54:38
 Last Modification Date: 2026-10-21 21:11:22

*/

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Charger : Enemy
{

    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _contactDamage = 20f;
    [SerializeField] private float _attackCooldown = 1f;
    private float _nextAttackTime;
    private NavMeshAgent _agent;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;
    }
    
    protected override void Behave()
    {

        if (!_agent.isOnNavMesh) return;

        if (!IsPlayerInRange())
        {
            _agent.isStopped = true;
            return;
        }
        
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);
        
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_nextAttackTime >= Time.time) return;

        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(_contactDamage);
        }

        _nextAttackTime = Time.time + _attackCooldown;
    }
}
