using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteToggle : MonoBehaviour
{
#pragma warning disable 0649
    [Header("прокачка Satelliet")]
    [SerializeField] private float startSatelliteSpeed;
    [SerializeField] private float multipleValue;
    [SerializeField] private int lvlSatelliteSkill;
    [SerializeField] private float currentSatellkiteSpeed;// для моніторингу.
    [SerializeField] private int index; //  індекс в ListButtonsSkills -> ListButtons
    [SerializeField] private Text costText; // Вартість
    [SerializeField] private GameObject satellitePref;
    [Space(10)]
    [SerializeField] private bool isActive;
    [SerializeField] private Toggle toggleAutoPlay;
#pragma warning restore 0649

    private List<int> costSkillUse;
    private GameObject currentSattellite;

    private void OnDestroy()
    {
        SkillsUpgrade.Instance.UpgradeSkills -= ChangeParameters;
        SkillsContainer.Instance.UpdateSkillsCost -= ShowCostSkiilUse;
    }

    private void OnEnable()
    {
        SkillsUpgrade.Instance.UpgradeSkills += ChangeParameters;
        SkillsContainer.Instance.UpdateSkillsCost += ShowCostSkiilUse;

        try
        {
            currentSattellite = FindObjectOfType<Satellite>().gameObject;
        }
        catch
        {

        }
        

        isActive = GameC.Instance.GetData().OnSatellite;
        toggleAutoPlay.isOn = GameC.Instance.GetData().OnSatellite;

        SkillsContainer.Instance.UpdateCostSkills();

        if (isActive)
            CreateSatellite(true);
    }

    public void ShowCostSkiilUse()
    {
        //відобразили вартівсть
        costSkillUse = SkillsUpgrade.Instance.GetCurrentCostSkillUse();
        costText.text = "-" + costSkillUse[index].ToString();
    }

    public void CreateSatellite(bool f)
    {
        GameC.Instance.GetData().OnSatellite = f;

        if (f && isActive)
        {
            ChangeParameters();

            if (currentSattellite != null)
            {
                Destroy(currentSattellite);
            }

            currentSattellite = Instantiate(satellitePref);

            //PointsCollector.Instance.SubPoints(costSkillUse[index]);
            //AnalyticsReport.Instance.UseSkill("Satellite");
            currentSattellite.GetComponentInChildren<Satellite>().Init(currentSatellkiteSpeed, index);         
        }
        else
            if (currentSattellite != null)
                Destroy(currentSattellite.gameObject);
    }

    /// <summary>
    /// Міняєм скорість обертання в залежності від рівня прокачки.
    /// </summary>
    private void ChangeParameters()
    {
        lvlSatelliteSkill = GameC.Instance.GetData().LvlsSkills[index];

        float css = startSatelliteSpeed;

        //який рівень скіла стільки й раз вдосконалюєм
        for (int i = 0; i < lvlSatelliteSkill; i++)
            css *= multipleValue;

        currentSatellkiteSpeed = css;

        if (currentSattellite != null)
            currentSattellite.GetComponentInChildren<Satellite>().SetSpeedRotation(currentSatellkiteSpeed);
    }
}
