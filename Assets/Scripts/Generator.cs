/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-21 19:29:50
 Last Modification Date: 2026-10-23 00:34:28

*/

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Generator : MonoBehaviour, IDamagable, IHealth
{
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;
    public bool IsDestroyed{get; private set;}
    public event System.Action Destroyed;
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public event System.Action<float, float> HealthChanged;

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDestroyed) return;
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(0f, _currentHealth);
        Debug.Log($"{name} recibió {damage} de daño. Vida: {_currentHealth}/{_maxHealth}");
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
        if (_currentHealth <= 0f)
        {
            OnDestroyed();
        }
    }

    private void OnDestroyed()
    {
        Debug.Log("Generador destruido");
        IsDestroyed = true;
        Destroyed?.Invoke();
        GetComponent<Renderer>().material.color = Color.black;
    }


}