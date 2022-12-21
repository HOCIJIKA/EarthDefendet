using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointsCollector : MonoBehaviour
{
#pragma warning disable 0649
    public int AllPoints; // загальна кількість очок
    public int LvlPoints; // очки набрані на рівні, для множника за рекламу.
    [Space(10)]
    [SerializeField] private GameObject ShowPrefPoints;
    [SerializeField] private int numberToStartMultipPoints; 
#pragma warning restore 0649

    private GameC gameC;
    private MenuUI menuUI;

    //GameC.Instance.SaveLoadSystem.Data.Settings.SoundEnabled = soundEnable; задаєм параметр
    //GameC.Instance.SaveLoadSystem.Save(); // зберігаєм

    private static PointsCollector instance;
    public static PointsCollector Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void Init(GameC gC)
    {
        
        gameC = gC;
        menuUI = gC.MenuUI;
        AllPoints = GameC.Instance.GetData().AllPoints;

        ShowAllPoints(AllPoints);
    }
    /// <summary>
    /// Добавляєм очки і задаєм позицію для відображення анімації очок
    /// </summary>
    /// <param name="points"></param>
    /// <param name="pos"></param>
    public void AddPoints(int points, Transform pos)
    {
        if (pos != null)
        {
            var animPoints = Instantiate(ShowPrefPoints, pos.position, transform.rotation);

            //Визначаєм множник
            var countMult = MultiPoints.Instance.NumberMult;
            countMult = (numberToStartMultipPoints >= countMult) ? 1 : countMult;

            if (countMult > 10)
                countMult = 10;
            if (countMult < 1)
                countMult = 1;

            // відображаєм очки
            animPoints.GetComponent<AnimPoints>().SetPointToShow(points * countMult, countMult);
            Destroy(animPoints, 1);

            LvlPoints += points * countMult;

            ShowAllPoints(LvlPoints + AllPoints);
            // Нарахували очки в статистику.
            LvlStatistic.Instance.AddStats(1, LvlStatistic.Instance.GetValuStats(1) + points * countMult);

            GameC.Instance.GetData().AllPoints = AllPoints + LvlPoints;
            GameC.Instance.SaveData();

            var allContainers = SkillsContainer.Instance.SkillsContainers;

            for (int i = 0; i < allContainers.Count; i++)
            {
                allContainers[i].GetComponent<Container>().ActiveButtonUp();
            }
        }
        else
        {
            // нагорода за захід
            GameC.Instance.GetData().AllPoints = AllPoints + points;
            GameC.Instance.SaveData();
            AllPoints = GameC.Instance.GetData().AllPoints;
            ShowAllPoints(GameC.Instance.GetData().AllPoints);
        }

        GPGSManager.Instance.CollAchievementOfPoints(GameC.Instance.GetData().AllPoints);
        GPGSManager.Instance.PostScoreToLeaderboard(GameC.Instance.GetData().AllPoints);

        SkillsContainer.Instance.UnlockContainers(GetPoints());
    }
    
    public int GetPoints()
    {
        return AllPoints + LvlPoints;
    }

    public void SubPoints(int i)
    {
        //потратили очки
        LvlStatistic.Instance.AddStats(2, LvlStatistic.Instance.GetValuStats(2) + i); 

        var curentPoints = GetPoints() - i;
        LvlPoints -= i;

        if (LvlPoints < 0 && AllPoints < 0)
            return;

        if (LvlPoints < 0)
        {
            LvlPoints = AllPoints - curentPoints;
            AllPoints -= LvlPoints;
            LvlPoints = 0;
        }

        if (AllPoints < 0)
        {
            AllPoints = 0;
            curentPoints = 0;
        }

        ShowAllPoints(curentPoints);

        GameC.Instance.GetData().AllPoints = curentPoints;
        GameC.Instance.SaveData();
    }

    public void ShowAllPoints(int p)
    {
        menuUI.ShowAllPoints(p);
    }
}
