using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Image _fill;
    private IHealth _health;

    void Awake()
    {
        if (_target == null)
        {
            Debug.LogError("No hay _target en HealthBar");
            return;
        }
        _health = _target.GetComponent<IHealth>();
        if (_health == null)
        {
            Debug.LogError("No hay IHealth en _target");
            return;
        }
    }

    void OnEnable()
    {
        if (_health == null)
        {
            return;
        }
        _health.HealthChanged += Refresh;
    }

    void OnDisable()
    {
        if (_health == null)
        {
            return;
        }
        _health.HealthChanged -= Refresh;
    }

    void Start()
    {
        if (_health == null)
        {
            return;
        }
        Refresh(_health.CurrentHealth, _health.MaxHealth);
    }

    private void Refresh(float current, float max)
    {
        if (max <= 0f) return;
        _fill.fillAmount = current / max;
    }

}
