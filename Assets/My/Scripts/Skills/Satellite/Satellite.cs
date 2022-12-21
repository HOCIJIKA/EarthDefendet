using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
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
    [SerializeField] private float speedRotation; // для моніторинку
    [SerializeField] private float timeDelayShoot;
#pragma warning restore 0649

    /// <summary>
    /// Індекс для провірки множення очків. 
    /// </summary>
    private int rocketIndex;
    private Shooter shooter;
    private bool detectedMeteor;
    private int skillIndex;

    private static Satellite instance;
    public static Satellite Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init(float sr, int indx)
    {
        shooter = FindObjectOfType<Shooter>();
        StartCoroutine(ShotRocket());
        SetSpeedRotation(sr);
        transform.rotation = new Quaternion(0, 0, Random.Range(0,360), Random.Range(-360, 360));

        skillIndex = indx;
    }

    private void Update()
    {
        // крутим пушку
        transform.Rotate(new Vector3(0, 0, 360) * Time.deltaTime * speedRotation);
    }

    private IEnumerator ShotRocket()
    {
        if (detectedMeteor)
        {
            if (PointsCollector.Instance.GetPoints() - SkillsUpgrade.Instance.GetCurrentCostSkillUse()[skillIndex] >= 0)
            {
                // якщо достятньо коштів для стрільби
                rocketIndex = shooter.GetRocketIndex();
                var curentRcket = Instantiate(rocket, StartPoint.position, StartPoint.rotation, rocketsContainer);
                var corentRocketPath = Instantiate(rocketPath, curentRcket.transform.position, curentRcket.transform.rotation, rocketsContainer);
                curentRcket.GetComponent<Rocket>().Init(rocketSpeed, rocketDamage, exsplosion, rocketIndex, corentRocketPath.transform);

                //добавили +1 ракету в статистику
                LvlStatistic.Instance.AddStats(3, LvlStatistic.Instance.GetValuStats(3) + 1);

                PointsCollector.Instance.SubPoints(SkillsUpgrade.Instance.GetCurrentCostSkillUse()[skillIndex]); 
                yield return new WaitForSeconds(timeDelayShoot);
            }

            yield return new WaitForSeconds(timeDelayShoot);
        }
        else
        {
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(ShotRocket());

    }

    public void DetectMeteor(bool f)
    {
        detectedMeteor = f;
    }

    public void SetSpeedRotation(float speed)
    {
        speedRotation = speed;
    }
}
