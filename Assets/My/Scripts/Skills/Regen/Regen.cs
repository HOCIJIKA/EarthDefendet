using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Regen : MonoBehaviour
{
    
    [SerializeField] private int lvlRegenSkill;
    [SerializeField] private int index;
    [SerializeField] private Text costText; // Вартість
    [SerializeField] private float multipleValue;
    [SerializeField] private float pasivSpeedRegen; // скорість пасивної регенерації хп
    [SerializeField] private int PasivValueRegen; // кількісне значеня пасивної регенерації
    [Space(10)]

    [SerializeField] private bool isActive;
    [Space(10)]
    [SerializeField] private Toggle toggleAutoPlay;

    private float startHP;
    private float hitPoints;
    private List<int> costSkillUse;

    private void OnDestroy()
    {
        SkillsUpgrade.Instance.UpgradeSkills -= UpgrateSkill;
        SkillsContainer.Instance.UpdateSkillsCost -= ShowCostSkillUse;
    }

    private void OnEnable()
    {
        SkillsUpgrade.Instance.UpgradeSkills += UpgrateSkill;
        SkillsContainer.Instance.UpdateSkillsCost += ShowCostSkillUse;

        isActive = true;
        toggleAutoPlay.isOn = isActive;

        startHP = Earth.Instance.GetStartHP();
        hitPoints = Earth.Instance.GetCurentHP();

        StartCoroutine(PasivRegeneration());
        UpgrateSkill();

        SkillsContainer.Instance.UpdateCostSkills();
    }

    public void UpgrateSkill()
    {
        // відклкик на обновлення скілів
        if (lvlRegenSkill != GameC.Instance.GetData().LvlsSkills[2])
        {
            lvlRegenSkill = GameC.Instance.GetData().LvlsSkills[2];
            ChangeParameters(lvlRegenSkill);
        }   
    }

    /// <summary>
    /// Міняєм параметри регенерації в залежності від рівня прокачки.
    /// </summary>
    /// <param name="lvl"></param>
    private void ChangeParameters(int lvl)
    {
        float v = 1;

        for (int i = 0; i < lvl; i++)
            v *= multipleValue;

        pasivSpeedRegen /= v; 
    }

    public void Activate(bool state)
    {
        isActive = state;
    }

    private void ShowCostSkillUse()
    {
        //відобразили вартівсть
        costSkillUse = SkillsUpgrade.Instance.GetCurrentCostSkillUse();
        costText.text = "-" + costSkillUse[index].ToString();
    }

    private IEnumerator PasivRegeneration()
    {
        yield return new WaitForSeconds(pasivSpeedRegen);
        hitPoints = Earth.Instance.GetCurentHP();

        if (hitPoints < startHP && PointsCollector.Instance.GetPoints() > costSkillUse[index])
        {
            Earth.Instance.AdHP(PasivValueRegen);
            LvlStatistic.Instance.AddStats(7, LvlStatistic.Instance.GetValuStats(7) + 1);
            // віднімаєм очки за регенку.
            PointsCollector.Instance.SubPoints(costSkillUse[index]);
        }

        StartCoroutine(PasivRegeneration());
    }
}
