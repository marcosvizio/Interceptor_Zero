using UnityEngine;
using TMPro;

public class GeneratorCounterUI : MonoBehaviour
{
    [SerializeField] private LevelExit _levelExit;
    [SerializeField] private TMP_Text _label;

    void OnEnable()
    {
        if (_levelExit == null) return;

        _levelExit.GeneratorsChanged += Refresh;

    }

    void OnDisable()
    {
        if (_levelExit == null) return;

        _levelExit.GeneratorsChanged -= Refresh;
    }

    void Refresh(int destroyed, int total)
    {
        if(_label == null) return;

        if (destroyed >= total)
        {
            _label.text = "Salida Abierta!";
            return;
        }

        _label.text = $"Generadores: {destroyed}/{total}";
    }


}
