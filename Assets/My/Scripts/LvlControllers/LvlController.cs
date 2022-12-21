using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlController : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Для моніторинку. Скільки метеоритів залишилось до кінця хвиді.")]
    /// <summary>
    /// для реєстрації метеоритів
    /// </summary>
    [SerializeField] private int countMeteors;
    [Header("Поточний рівень")]
    [SerializeField] private int currentLvl;
    [Header("Налаштування рівня")]
    [Tooltip("задається раз")]
    [SerializeField] private int startCountMeteors;
    [Tooltip("В перерахунку з балансом віносно поточнго рівня")]
    [SerializeField] private int currentCountMeteors;
#pragma warning restore 0649

    private static LvlController instance;
    public static LvlController Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        currentLvl = GameC.Instance.GetData().CurrentLvl;
        currentCountMeteors = startCountMeteors + currentLvl;
        SpamMeteor.Instance.Init(currentCountMeteors);
        SkillsContainer.Instance.UpdateCostSkills();

        GPGSManager.Instance.PostLevelToLeaderboard(currentLvl);

        try
        {
            FirebaseManager.Instance.SaveUserMaxLevel(currentLvl);
            FirebaseManager.Instance.SaveUserMaxPoints(PointsCollector.Instance.AllPoints);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }

    public void ChangeCountMeteors(int i)
    {
        countMeteors += i;

        if (countMeteors <= 0)
        {
            // добавити якість ефекти при перемозі
            Win();
        }
    }

    /// <summary>
    /// Метеорити закінчились, гравець пережив хвилю.
    /// </summary>
    private void Win()
    {
        var containers = FindObjectsOfType<Container>();
        StartCoroutine(DelayStatisticPanel(containers));
    }

    private IEnumerator DelayStatisticPanel(Container[] containers)
    {
        yield return new WaitUntil(() =>
        {

            foreach (var container in containers)
            {
                if (container.ButtonHideList.activeSelf)
                {
                    container.ButtonHideList.SetActive(false);
                    container.ButtonBuy.ButtonInteractable(false);
                }
            }

            foreach (var container in containers)
            {
                if (container.ListAllSkills.activeSelf)
                {
                    MenuUI.Instance.SowSelectSkillText();
                    return false;
                }
            }

            MenuUI.Instance.HideSelectSkillText();
            return true;
        });
        
        try
        {
            LvlStatistic.Instance.ShowStatistic();
            Debug.Log("WINER!");
            GameC.Instance.GetData().CurrentLvl += 1;
        }
        catch
        { 
        }
    }

    /// <summary>
    /// Наступна хвиля. Викликається з кновки в едіроті. на панелі статистики(можливо переробити) 
    /// </summary>
    public void NextLvl()
    {
        //AnalyticsReport.Instance.StartLvl(GameC.Instance.GetData().CurrentLvl);

        GameC.Instance.SaveData();
        SceneManager.LoadScene(0);
    }

    public void Loss()
    {
        Advertising.Instance.ShowInterstitialRestartLvl();
        MenuUI.Instance.OpenLoss(true);
        Time.timeScale = 0;
    }

    public void RestartLvl()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


}
