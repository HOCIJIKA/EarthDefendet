using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject exsplosion;
    [Space(10)]
    [Header("Настройки ракети")]
    [SerializeField] private float rocketSpeed;
    [SerializeField] private float rocketDamage;
    [Header("Настройки стрілка")]
    [SerializeField] private GameObject rocket;
    [SerializeField] private GameObject rocketPath;
    [SerializeField] private Transform rocketsContainer;
    [SerializeField] private Transform StartPoint;
    [SerializeField] private int rocketCoutn;
    [SerializeField] private int regenValueRocketCoutn;
    [SerializeField] private float regenSpeedRocketCoutn;
    [Range(0, 1)]
    [SerializeField] private float speedRotation;
#pragma warning restore 0649

    private float randomDiraction;
    /// <summary>
    /// Індекс для провірки множення очків. 
    /// </summary>
    private int rocketIndex;

    private static Shooter instance; // свойство через яке мы можему звернутись звідусіль.
    public static Shooter Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init() // пізніше засунути в Init(),  коли будут рівні.
    {
        SetDiraction();
        StartCoroutine(RegenRocketCount());
    }

    private void Update()
    {
        // крутим пушку
        transform.Rotate(new Vector3(0, 0, randomDiraction * 360) * Time.deltaTime * speedRotation);
    }

    private void SetDiraction()
    {
        //randomDiraction = Random.Range(-1, 1);
        //if (randomDiraction >= 0) randomDiraction = -1;
        //else randomDiraction = 1;
        randomDiraction = -1;
    }

    public void ShotRocket()
    {
        if (rocketCoutn > 0)
        {
            rocketIndex++;
            var curentRcket = Instantiate(rocket, StartPoint.position, StartPoint.rotation, rocketsContainer);
            var corentRocketPath = Instantiate(rocketPath, curentRcket.transform.position, curentRcket.transform.rotation, rocketsContainer);
            curentRcket.GetComponent<Rocket>().Init(rocketSpeed, rocketDamage, exsplosion, rocketIndex, corentRocketPath.transform);
            rocketCoutn--;
            MenuUI.Instance.ShowRocketCount(rocketCoutn);

            //добавили +1 ракету в статистику
            LvlStatistic.Instance.AddStats(3, LvlStatistic.Instance.GetValuStats(3) + 1);
        }
        else
        {
            //Debug.Log("ракети закінчились");
        }
    }

    IEnumerator RegenRocketCount()
    {
        yield return new WaitForSeconds(regenSpeedRocketCoutn);
        if (true) // покищо так, далі можна задати максимальну кількість ракет.
        {
            rocketCoutn += regenValueRocketCoutn;
            MenuUI.Instance.ShowRocketCount(rocketCoutn);
        }
        StartCoroutine(RegenRocketCount());
    }

    public int GetRocketIndex()
    {
        return rocketIndex;
    }
}

