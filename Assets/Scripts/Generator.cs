using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Generator : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;

    public bool IsDestroyed{get; private set;}
    public event System.Action Destroyed;

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