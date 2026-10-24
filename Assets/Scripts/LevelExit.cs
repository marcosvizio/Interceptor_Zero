using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    
    [SerializeField] private GameObject _barriers;
    [SerializeField] private Generator[] _generators;
    [SerializeField] private string _victorySceneName = "Victory";

    private int _remaining;
    private int _total;
    private bool _levelCompleted;
    private bool _isOpen = false;
    public IReadOnlyList<Generator> Generators => _generators;

    public event System.Action<int, int> GeneratorsChanged;

    void Start()
    {
        _remaining = 0;
        
        foreach (Generator generator in _generators)
        {
            if (generator==null)
            {
                continue;
            }
            generator.Destroyed += OnGeneratorDestroyed;
            _remaining++;
        }

        _total = _remaining;

        if (_remaining <= 0)
        {
            Open();
        }

        NotifyGeneratorsChanged(); 
    }

    void OnGeneratorDestroyed()
    {
        _remaining -= 1;
        NotifyGeneratorsChanged();
        if (_remaining <= 0)
        {
            Open();
        }
    }

   private void Open()
    {
        _isOpen = true;

        if (_barriers != null)
        {
            _barriers.SetActive(false);
        }

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
        SceneManager.LoadScene(_victorySceneName);
    }

    private void NotifyGeneratorsChanged()
    {
        int destroyed = _total - _remaining;
        GeneratorsChanged?.Invoke(destroyed, _total);
    }
}
