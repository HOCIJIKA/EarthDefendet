using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Text _number;
    [SerializeField] private Text _name;
    [SerializeField] private Text _points;
    [SerializeField] private Text _level;

    public void SetValues(int number, string name, string points, string level)
    {
        _number.text = number.ToString();
        _name.text = name;
        _points.text = points;
        _level.text = level;
    }
}
