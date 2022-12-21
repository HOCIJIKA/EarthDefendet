using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveButton : MonoBehaviour
{
#pragma warning disable 0649
    [Header("прокачка Хвилі")]
    [SerializeField] private float multipleValue;
    [SerializeField] private float startWaveScale;
    [SerializeField] private int lvlWaveSkill;
    [SerializeField] private float curentsWaveScale;// для моніторингу.
    [Space(10)]
    [SerializeField] private int index;// індекс в ListButtonsSkills -> ListButtons
    [SerializeField] private Text costText; // Вартість
    [SerializeField] private GameObject wavePref;
    [SerializeField] private float damage;
#pragma warning restore 0649

    private List<int> costSkillUse;

    private Vector3 scaleWaveFromLvl;

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

    private void ShowCostSkillUse()
    {
        //відобразили вартівсть
        costSkillUse = SkillsUpgrade.Instance.GetCurrentCostSkillUse();
        costText.text = "-" + costSkillUse[index].ToString();
    }

    public void CreateWave()
    {
        ChangeParameters();

        var points = PointsCollector.Instance.GetPoints();

        if (points > costSkillUse[index])
        {
            var newWave = Instantiate(wavePref);
            newWave.transform.localScale = scaleWaveFromLvl;
            newWave.GetComponentInChildren<Wave>().Init(damage);

            PointsCollector.Instance.SubPoints(costSkillUse[index]);
        }
    }

    /// <summary>
    /// Міняєм параметри Хвилі в залежності від рівня прокачки.
    /// </summary>
    private void ChangeParameters()
    {
        lvlWaveSkill = GameC.Instance.GetData().LvlsSkills[index];

        float sws = startWaveScale;

        //який рівень скіла стільки й раз вдосконалюєм
        for (int i = 0; i < lvlWaveSkill; i++)
            sws *= multipleValue;

        scaleWaveFromLvl = new Vector3(sws, sws, sws);
        curentsWaveScale = sws;
    }
}
