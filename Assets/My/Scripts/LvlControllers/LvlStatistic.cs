using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LvlStatistic : MonoBehaviour
{
#pragma warning disable 0649
    [Header("NextButton")] [SerializeField]
    private bool OpenNextButtonInEndStats;

    [SerializeField] private float timeDelayNextButton;
    [SerializeField] private GameObject nextButton;

    [Header("Час затримки статистики")] [SerializeField]
    private float TimeDelay;

    [Space(10)] [SerializeField] int multipleValuAds = 2;

    /// <summary>
    /// 0-яка зараз хвиля, 1-зароблено очок на рівні, 2-Очки витрачені на навички, 
    /// 3-скільки ракет вистілено за лвл, 4-скільки ракет попало в мішень,
    /// 5-Загальний урон вміннями та ракетами, 6-Отриманий урон, 7-Скільки відновили HP.
    /// </summary>
    public List<int> StatisticValue;

    [Header("Панель статистики")] public GameObject StatisticPanel;

    [Header("Тексти")] [SerializeField] private List<Text> ListText;
#pragma warning restore 0649

    private int n;

    private List<int> statsList = new List<int>();

    private static LvlStatistic _instance;
    public static LvlStatistic Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    /// <summary>
    /// Додаєм значення для статистики
    /// </summary>
    /// <param name="index">індекс значення в масиві</param>
    /// <param name="count">значення</param>
    public void AddStats(int index, int count)
    {
        StatisticValue[index] = count;
    }

    /// <summary>
    /// отримуєм значення зі списку статистики для додавання
    /// </summary>
    /// <param name="index">індекс значення в масиві</param>
    /// <returns></returns>
    public int GetValuStats(int index)
    {
        return StatisticValue[index];
    }

    /// <summary>
    /// Відкриває статистику і процес нарахування
    /// </summary>
    public void ShowStatistic()
    {
        AddStats(0, GameC.Instance.GetData().CurrentLvl);
        StartCoroutine(DelayShowStatistic());
    }

    private IEnumerator DelayShowStatistic()
    {
        yield return new WaitForSeconds(TimeDelay);
        StatisticPanel.SetActive(true);
        ShowNextButton();

        for (int i = 0; i < StatisticValue.Count; i++)
            statsList.Add(0);

        StartCoroutine(SetDataStats());
    }

    private void ShowNextButton()
    {
        StartCoroutine(DelayNextButton());
    }

    private IEnumerator DelayNextButton()
    {
        yield return new WaitForSeconds(timeDelayNextButton);
        nextButton.SetActive(true);
    }

    private IEnumerator SetDataStats()
    {
        n++;
        for (int i = 0; i < StatisticValue.Count; i++)
        {
            if (statsList[i] != -1)
            {
                if (statsList[i] + 20 < StatisticValue[i])
                    statsList[i] += 20;
                else if (statsList[i] + 10 < StatisticValue[i])
                    statsList[i] += 10;
                else if (statsList[i] < StatisticValue[i])
                    statsList[i] += 1;
                FX.Instance.PlayAddPoints();
            }

            if (statsList[i] == StatisticValue[i])
                statsList[i] = -1;

            ShowStats();
        }

        if (n == StatisticValue.Count && OpenNextButtonInEndStats)
        {
            nextButton.SetActive(true);
        }

        yield return new WaitForSeconds(0.000001f);
        StartCoroutine(SetDataStats());
    }

    private void ShowStats()
    {
        for (int i = 0; i < StatisticValue.Count; i++)
            if (statsList[i] > 0)
                ListText[i].text = (statsList[i]).ToString();
    }

    /// <summary>
    /// Викликається з кнопки Next коли потрібно очистити статистику
    /// </summary>
    public void ClearStats()
    {
        for (int i = 0; i < StatisticValue.Count; i++)
            StatisticValue[i] = 0;

        StatisticPanel.SetActive(false);
        n = 0;
    }

    public void MultiplierPointsAds()
    {
        Advertising.Instance.ShowRewardedMultipleCoins();
        StatisticValue[1] *= multipleValuAds;
        ListText[1].text = (StatisticValue[1]).ToString();
        AddStats(1, StatisticValue[1]);
        GameC.Instance.GetData().AllPoints -= StatisticValue[1] / multipleValuAds;
        GameC.Instance.GetData().AllPoints += StatisticValue[1];
        GameC.Instance.SaveData();
        MenuUI.Instance.DeactivateAddPointsAdsButton();
        Debug.Log(" Multiplier Points");
    }
}