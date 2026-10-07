/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-06 23:53:54
 Last Modification Date: 2026-10-07 19:24:32

*/

using UnityEngine;

public abstract class Character : MonoBehaviour, IDamagable
{

    [SerializeField] protected float _maxHealth = 100f;
    protected float _currentHealth;

    
    public bool IsDead { get; private set; }

    protected virtual void Awake()
    {
        ResetHealth();
    }
    
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        _currentHealth -= damage;

        _currentHealth = Mathf.Max(0f, _currentHealth);

        Debug.Log($"{name} recibió {damage} de daño. Vida: {_currentHealth}/{_maxHealth}");

        if(_currentHealth <= 0)
        {
            IsDead = true;
            Die();  
        } 
    }

    protected void ResetHealth()
    {
        _currentHealth = _maxHealth;
        IsDead = false;
    }

    protected abstract void Die();
}