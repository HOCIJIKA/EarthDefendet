using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Earth : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float hitPoints; // життя планети
#pragma warning restore 0649

    private float startHP;

    private MenuUI menuUI;

    private static Earth instance;
    public static Earth Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init(MenuUI MUI)
    {
        startHP = hitPoints;
        menuUI = MUI;
        menuUI.SetEarthSliderMaxValue(hitPoints);
        menuUI.SetEarthSliderValue(hitPoints);
    }

    private void EndHP()// кінець життя планети
    {
    }

    public void SetDamage(int d)
    {
        hitPoints -= d;
        LvlStatistic.Instance.AddStats(6, LvlStatistic.Instance.GetValuStats(6) + d);
        ReportHP();

        if (hitPoints <= 0)
            LvlController.Instance.Loss();
    }

    private void ReportHP()
    {
        menuUI.SetEarthSliderValue(hitPoints);
    }

    public void AdHP(float v)
    {
        hitPoints += v;
        ReportHP();
    }

    public float GetStartHP()
    {
        return startHP;
    }

    public float GetCurentHP()
    {
        return hitPoints;
    }
}
