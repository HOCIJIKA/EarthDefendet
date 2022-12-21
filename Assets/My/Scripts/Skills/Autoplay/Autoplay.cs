using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Autoplay : MonoBehaviour
{
#pragma warning disable 0649
    [Header("прокачка автопілата")]
    [SerializeField] private float multipleValue;
    [SerializeField] private int lvlAutoPlaySkill;
    [SerializeField] private GameObject arrowParent;
    [Header("Настройки AutoPlay")]
    [SerializeField] private int index;
    [SerializeField] private Text costText; // Вартість
    [SerializeField] private bool detectedMeteor; // метеорит в зоні Arrow.
    [SerializeField] private float delayAutoPlay;
    [Space(10)]
    [SerializeField] private bool isActive;
    [Space (10)]
    [SerializeField] private Toggle toggleAutoPlay;
#pragma warning restore 0649

    private List<int> costSkillUse;

    private void OnDestroy()
    {
        SkillsUpgrade.Instance.UpgradeSkills -= UpgradeSkill;
        SkillsContainer.Instance.UpdateSkillsCost -= ShowCostSkillUse;
    }

    private void OnEnable()
    {
        SkillsUpgrade.Instance.UpgradeSkills += UpgradeSkill;
        SkillsContainer.Instance.UpdateSkillsCost += ShowCostSkillUse;

        lvlAutoPlaySkill = GameC.Instance.GetData().LvlsSkills[0];

        //задаєм параметри для стрілки відносно рівня
        ChangeParameters(lvlAutoPlaySkill);

        isActive = GameC.Instance.GetData().Autoplay;
        toggleAutoPlay.isOn = GameC.Instance.GetData().Autoplay;
        StartCoroutine(AutoShot());

        Arrow.Instance.SetAutoPlay(this);
        SkillsContainer.Instance.UpdateCostSkills();
    }

    public void UpgradeSkill()
    {
        // відклкик на обновлення скілів
        if (lvlAutoPlaySkill != GameC.Instance.GetData().LvlsSkills[0])
        {
            lvlAutoPlaySkill = GameC.Instance.GetData().LvlsSkills[0];
            ChangeParameters(lvlAutoPlaySkill);
        }
    }

    /// <summary>
    /// Міняєм параметри автопілота в залежності від рівня прокачки.
    /// </summary>
    /// <param name="lvl"></param>
    private void ChangeParameters(int lvl)
    {     
        arrowParent.transform.localScale = new Vector3(1, 1, 1);

        float v = 1;

        for (int i = 0; i < lvl; i++)
        {
            v *= multipleValue;
        }

        arrowParent.transform.localScale = new Vector3(1, v, 1);
        arrowParent.GetComponentInChildren<Arrow>().SetDistandeToShot(v);
    }

    public void SetValue(bool state)
    {
        isActive = state;
        GameC.Instance.GetData().Autoplay = state;
    }

    private void ShowCostSkillUse()
    {
        //відобразили вартівсть
        costSkillUse = SkillsUpgrade.Instance.GetCurrentCostSkillUse();
        costText.text = "-" + costSkillUse[index].ToString();
    }

    private IEnumerator AutoShot()
    {
        if (detectedMeteor && isActive)
        {
            if (PointsCollector.Instance.GetPoints() - SkillsUpgrade.Instance.GetCurrentCostSkillUse()[index] >= 0)
            {
                // якщо достятньо коштів для стрільби
                Shot();
                PointsCollector.Instance.SubPoints(costSkillUse[index]);
            }
            else
            {
                // можна добавити якись противний звукчи щось таке.
                Debug.Log(string.Format(" недостатньо коштів для автострільби, в гравця: {0}", PointsCollector.Instance.GetPoints()));
            }

            yield return new WaitForSeconds(delayAutoPlay);
        }
        else
        {
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(AutoShot());
    }

    private void Shot()
    {
        Shooter.Instance.ShotRocket();
    }

    public void DetectMeteor(bool d)
    {
        detectedMeteor = d;
    }
}
