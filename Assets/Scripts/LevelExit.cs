using UnityEngine;

public class LevelExit : MonoBehaviour
{
    
    private bool _isOpen = false;
    [SerializeField] private GameObject _barriers;
    [SerializeField] private Generator[] _generators;

    private int _remaining;
    private bool _levelCompleted;

    void Start()
    {
        _remaining = _generators.Length;
        
        foreach (Generator generator in _generators)
        {
            if (generator==null)
            {
                continue;
            }
            generator.Destroyed += OnGeneratorDestroyed;
        }

        if (_remaining <= 0)
        {
            Open();
        } 
    }

    void OnGeneratorDestroyed()
    {
        _remaining -= 1;
        Debug.Log($"Quedan {_remaining} generadores por romper.");
        if (_remaining <= 0)
        {
            Open();
        }
    }

    private void Open()
    {
        if (_barriers != null) return;
        _isOpen = true;
        _barriers.SetActive(false);
        Debug.Log("La salida esta abierta!");
    }

    void OnDestroy()
    {
        foreach (Generator generator in _generators)
        {
            if (generator==null)
            {
                continue;
            }
            generator.Destroyed -= OnGeneratorDestroyed;
        }
    }    

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        if(!_isOpen) return;
        if(_levelCompleted) return;
        _levelCompleted = true;
        Debug.Log("Nivel Completado!");
    }
}
