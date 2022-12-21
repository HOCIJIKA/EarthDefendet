using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreePoints : MonoBehaviour
{
    [SerializeField] private int countFreePoints;
    [SerializeField] private Text countFreePointsText;
    [SerializeField] private Text countMultiText;
    [Space(10)]
    [SerializeField] private GameObject freePointsPanel;
    [SerializeField] private Button freePointsButton;
    [Space(10)]
    [SerializeField] private int countMin;
    [SerializeField] private Text timerText;
    [Space(10)]
    [SerializeField] private System.DateTime curentDataTime;

    private int countMultipleFreePoints = 1;
    private System.DateTime freePointsData;

    private static FreePoints instance;
    public static FreePoints Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        if (GameC.Instance.GetData().Day == 0)
        {
            // так якд день не може бути 0, значить зайшли вперше.
            SetNewTimeFree();
            GameC.Instance.SaveData();
        }
        else
        {
            freePointsData = new System.DateTime(System.DateTime.Now.Year,
                System.DateTime.Now.Month,
                GameC.Instance.GetData().Day,
                GameC.Instance.GetData().Hour,
                GameC.Instance.GetData().Minute,
                GameC.Instance.GetData().Second);

            freePointsData = freePointsData.AddMinutes(countMin);

            if (GameC.Instance.GetData().countMultipleFreePoints != 0)
                countMultipleFreePoints = GameC.Instance.GetData().countMultipleFreePoints;

            if (freePointsData.Day < System.DateTime.Now.Day)
            {
                // якщо настав новий день
                GameC.Instance.GetData().countMultipleFreePoints = 1;
                countMultipleFreePoints = GameC.Instance.GetData().countMultipleFreePoints;
                countMultiText.text = $"{countFreePoints} X{countMultipleFreePoints + 1}";
            }

            if (freePointsData < System.DateTime.Now)
                OpenFreePanel();
        }

        StartCoroutine(Timer());
    }

    private void SetNewTimeFree()
    {
        GameC.Instance.GetData().Day = System.DateTime.Now.Day;
        GameC.Instance.GetData().Hour = System.DateTime.Now.Hour;
        GameC.Instance.GetData().Minute = System.DateTime.Now.Minute;
        GameC.Instance.GetData().Second = System.DateTime.Now.Second;

        GameC.Instance.SaveData();

        freePointsData = new System.DateTime(System.DateTime.Now.Year,
            System.DateTime.Now.Month,
            GameC.Instance.GetData().Day,
            GameC.Instance.GetData().Hour,
            GameC.Instance.GetData().Minute,
            GameC.Instance.GetData().Second);

        freePointsData = freePointsData.AddMinutes(countMin);
    }

    private void AddFreeMultiple()
    {
        countMultipleFreePoints = GameC.Instance.GetData().countMultipleFreePoints += 1;
        GameC.Instance.SaveData();
    }

    private void OpenFreePanel()
    {
        freePointsPanel.SetActive(true);
        countFreePointsText.text = (countFreePoints * countMultipleFreePoints).ToString();
        countMultiText.text = $"{countFreePoints} X{countMultipleFreePoints + 1}";
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);

        if (freePointsData > System.DateTime.Now)
        {
            var t = freePointsData - System.DateTime.Now;

            timerText.text = $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";

            if (t.Hours <= 0 && t.Minutes <= 0 && t.Seconds <= 0)
            {
                if (!freePointsPanel.activeSelf)
                    OpenFreePanel();

                countFreePointsText.text = (countFreePoints * countMultipleFreePoints).ToString();
                freePointsButton.interactable = true;
            }
            else
                freePointsButton.interactable = false;
        }

        StartCoroutine(Timer());
    }

    public void GetFree()
    {
        PointsCollector.Instance.AddPoints(countFreePoints * countMultipleFreePoints, null);
        freePointsPanel.SetActive(false);
        AddFreeMultiple();
        SetNewTimeFree();
        Debug.Log("Take free points :" + countFreePoints * countMultipleFreePoints);
    }
}
