using UnityEngine;

/// Отримує індекіси від ракет які попали в метеорит.
/// Якщо індекси ідуть послідовно, гравцю додаються очки.
public class MultiPoints : MonoBehaviour
{
    public static MultiPoints Instance => _instance;

    private int _numberMult;
    private int _newIndex;
    private int _oldIndex;
    private static MultiPoints _instance;

    public int NumberMult => _numberMult;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void CheckIndexSequence(int index)
    {
        _newIndex = index;
        if (_oldIndex +1 == _newIndex)
        {
            _oldIndex = _newIndex;
            _numberMult++;

        }
        else
        {
            _oldIndex = _newIndex;
            _numberMult = 1;
        }
    }

    public int GetNumberMult()
    {
        return _numberMult;
    }
}
