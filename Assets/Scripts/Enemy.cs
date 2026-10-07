/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-06 23:54:14
 Last Modification Date: 2026-10-07 18:55:35

*/

using UnityEngine;

public abstract class Enemy : Character
{
    protected Transform _player;
    [SerializeField] protected float _detectionRange = 10f;
    
    protected override void Awake()
    {
        base.Awake();
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject == null)
        {
            Debug.LogError("No hay Player para obtener su Transform.");
            return;
        }

        _player = playerObject.transform;
    }

    void Update()
    {
        if (IsDead || !_player) return;
        Behave();
    }

    protected bool IsPlayerInRange()
    {
        // devolvé true si la distancia entre el enemigo y el player es menor o igual a _detectionRange

        return Vector3.Distance(_player.position, transform.position) <= _detectionRange;

    }

    protected abstract void Behave();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }


    protected override void Die()
    {
        Destroy(gameObject);
    }
}
