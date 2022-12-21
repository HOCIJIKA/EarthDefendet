using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldButton : MonoBehaviour
{
#pragma warning disable 0649
    [Header("прокачка Shield")]
    [SerializeField] private float startShieldValue;
    [SerializeField] private float multipleValue;
    [SerializeField] private float curentsShieldValue;// для моніторингу.
    [SerializeField] private int lvlShieldSkill;
    [SerializeField] private int index; // індекс в ListButtonsSkills -> ListButtons
    [SerializeField] private Text costText; // Вартість
    [SerializeField] private GameObject shieldPref;
#pragma warning restore 0649

    private List<int> costSkillUse;

    private void OnDestroy()
    {
        SkillsUpgrade.Instance.UpgradeSkills -= ChangeParameters;
        SkillsContainer.Instance.UpdateSkillsCost -= ShowCostSkillUse;
    }

    private void OnEnable()
    {
        SkillsUpgrade.Instance.UpgradeSkills += ChangeParameters;
        SkillsContainer.Instance.UpdateSkillsCost += ShowCostSkillUse;

        SkillsContainer.Instance.UpdateCostSkills();
    }

    public void ShowCostSkillUse()
    {
        //відобразили вартівсть
        costSkillUse = SkillsUpgrade.Instance.GetCurrentCostSkillUse();
        costText.text = "-" + costSkillUse[index].ToString();
    }

    public void CreateShield()
    {
        ChangeParameters();

        var points = PointsCollector.Instance.GetPoints();

        if (points > costSkillUse[index])
        {
            try
            {
                var a = FindObjectOfType<Shield>().gameObject;

                if (a != null)
                {
                    Destroy(a);
                }
            }
            catch(Exception e)
            {
                Debug.LogWarning(e);
            }
            

            var newShield = Instantiate(shieldPref);
            newShield.GetComponent<Shield>().Init(curentsShieldValue);
            
            PointsCollector.Instance.SubPoints(costSkillUse[index]);
        }
    }

    /// <summary>
    /// Міняєм параметри Щита в залежності від рівня прокачки.
    /// </summary>
    private void ChangeParameters()
    {
        lvlShieldSkill = GameC.Instance.GetData().LvlsSkills[index];

        float sws = startShieldValue;

        //який рівень скіла стільки й раз вдосконалюєм
        for (int i = 0; i < lvlShieldSkill; i++)
            sws *= multipleValue;

        curentsShieldValue = sws;
    }
}
