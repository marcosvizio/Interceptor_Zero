using UnityEngine;

public class LevelExit : MonoBehaviour
{
    
    [SerializeField] private bool _isOpen = true;
    private bool _levelCompleted;

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        if(!_isOpen) return;
        if(_levelCompleted) return;
        _levelCompleted = true;
        Debug.Log("Nivel Completado!");
    }
}
