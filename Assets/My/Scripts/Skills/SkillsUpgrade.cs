using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUpgrade : MonoBehaviour
{
    public delegate void UpgradeDelegane();
    public event UpgradeDelegane UpgradeSkills;

#pragma warning disable 0649
    [Header("На скільки вдосконалються скіли і вартість їх апгрейда")]
    [SerializeField] private float upValue; // покищо однакове значеня для всіх скілів і вдосконалень.
    [Tooltip("Кофіцієнт апгрейда скіла автопілот")]
    [SerializeField] private float upCostAutoPilUp; 
    [Tooltip("Кофіцієнт використання скіла автопілот")]
    [SerializeField] private float upUseAutoPil;
    [Tooltip("Кофіцієнт апгрейда скіла Wave")]
    [SerializeField] private float upCostWavelUp;
    [Tooltip("Кофіцієнт використання скіла Wave")]
    [SerializeField] private float upUseWave;
    [Tooltip("Кофіцієнт апгрейда скіла Wave")]
    [SerializeField] private float upCostRegenUp;
    [Tooltip("Кофіцієнт використання скіла Wave")]
    [SerializeField] private float upUseRegen;


    [Header("Стартова вартість використання скілів")]
    [SerializeField] private List<int> startCostSkillUse;
    [HideInInspector] [SerializeField] public List<int> currentCostSkillUse;
    [Header("Стартова вартітсть апгрейда скілів")]
    [SerializeField] private List<int> startCostSkillsUp;
    [Header("Поточна вартітсть апгрейда скілів")]
    [HideInInspector] [SerializeField] private List<int> currentCostSkillUp;
    [Header("Рівень прокачки скілів")]
    [SerializeField] public List<int> lvlsSkills;

#pragma warning restore 0649

    private static SkillsUpgrade instance;
    public static SkillsUpgrade Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        lvlsSkills = GameC.Instance.GetData().LvlsSkills;

        // кожного разу при старті гри обновляєм значення вартості скілів
        // для легкої модернізації балансу гри
        List<int> a = new List<int>();
        for (int i = 0; i < startCostSkillUse.Count; i++)
            a.Add(startCostSkillUse[i]);
        currentCostSkillUse = a;

        List<int> b = new List<int>();
        for (int i = 0; i < startCostSkillsUp.Count; i++)
            b.Add(startCostSkillsUp[i]);
        currentCostSkillUp = b;

        for (int i = 0; i < ListButtonsSkills.Instance.GetCountButtonsSkill(); i++)
            FirstStartUpgradeSkill(i);
    }

    public List<int> GetCurrentCostSkillUse()
    {
        return currentCostSkillUse;
    }

    public List<int> GetCurrentCostSkillUp()
    {
        return currentCostSkillUp;
    }

    public int GetValueSkillUp(int i)
    {
        return currentCostSkillUp[i];
    }

    /// <summary>
    /// Спрожена функція UpgradeSkill. Тобто без реального апгрейда
    /// </summary>
    /// <param name="si"></param>
    public void FirstStartUpgradeSkill(int si)
    {
        UpUpCost(si, lvlsSkills[si]);
        UpUseCost(si, lvlsSkills[si]);
        GameC.Instance.SaveData();
    }

    /// <summary>
    /// Апргнейдим і його вартість скіл за індексом
    /// </summary>
    /// <param name="si"> скіл індекс</param>
    public void UpgradeSkill(int si)
    {
        // підняли рівень скіла
        lvlsSkills[si]++;
        GameC.Instance.GetData().LvlsSkills = lvlsSkills;
        
        UpgradeSkills(); // сповістили всіх по обновлення рівнів скілів

        // поміняли дані про скіл
        UpUpCost(si, lvlsSkills[si]);
        UpUseCost(si, lvlsSkills[si]);

        GameC.Instance.SaveData();
    }

    private void UpUpCost(int index, int lvlSkill)
    {
        if (lvlSkill == 0)
            return;

        //індивідуально для кожного скіла
        switch (index)
        {
            case 0: // автопілот

                break;

            case 1: // Хвиля

                break;

            case 2: // Реген

                break;

            default:

                break;
        }

        var startCost = startCostSkillsUp[index];
        float value = startCost;

        for (int i = 0; i < lvlSkill ; i++)
            value *= upValue;
        startCost = (int)value;
        currentCostSkillUp[index] = startCost;

        // обновляєм тексти значення вартості апгрейда
        var containers = SkillsContainer.Instance.SkillsContainers;
        for (int i = 0; i < containers.Count; i++)
            containers[i].GetComponent<Container>().SetCostUp(currentCostSkillUp[i]);

        //Debug.Log(string.Format("Вартість використання скіла: {0}, піднялась на: {1}, і ={2}", index, (int)value - startCost, value));
    }

    private void UpUseCost(int index, int lvlSkill)
    {
        if (lvlSkill == 0)
            return;

        float value = startCostSkillUse[index]; 
        var startCost = value;

        for (int i = 0; i < lvlSkill; i++)
            value *= upValue;

        currentCostSkillUse[index] = (int)value;

        // обновляєм тексти значення вартості апгрейда
        var containers = SkillsContainer.Instance.SkillsContainers;
        for (int i = 0; i < containers.Count; i++)
            containers[i].GetComponent<Container>().SetCostUp(currentCostSkillUp[i]);

        //Debug.Log(string.Format("Вартість вдосконалення скіла: {0}, піднялась на: {1}, і ={2}", index, (int)value - value, value));      
    }


}
