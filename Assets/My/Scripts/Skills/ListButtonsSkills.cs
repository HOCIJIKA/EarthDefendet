using System.Collections.Generic;
using UnityEngine;

/// <summary>
///Cтворюєм екземпляр 'Instantiate()' потрібної кнопки і пеміщаєв її в "якір" контейнера
/// </summary>
public class ListButtonsSkills : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Кнопки всіх скілів")]
    public List<GameObject> ListButtons;
    [Header("Стартова вартість використання скілів")]
    [Tooltip("порядок в ListStartCostSkillUs  має = ListButtons")]
    public List<int> listStartCostSkillUs;
#pragma warning restore 0649

    private static ListButtonsSkills instance;
    public static ListButtonsSkills Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject GetButtonSkill(int number)
    {
        return ListButtons[number].gameObject;
    }

    public List<GameObject> GetListButtons()
    {
        return ListButtons;
    }

    public int GetCountButtonsSkill()
    {
        return ListButtons.Count;
    }

}
