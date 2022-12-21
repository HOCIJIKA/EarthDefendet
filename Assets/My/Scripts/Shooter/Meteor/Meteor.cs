using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Meteor : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float speedMove; //скорість руху
    [SerializeField] private int damage; // урон метеориту
    [SerializeField] private float hP; // життя метеориту
    [SerializeField] private GameObject particlesBoom;
#pragma warning restore 0649

    private GameObject earthObj;
    private PointsCollector pointsCollector;
    private float maxHP;
    private Vector3 targetMove;// координати землі
    private float distanceToEarh; // потрібно для визначеня кількості очок відносно відстані взриву метеориту.
    private Earth earthC;
    private List<GameObject> meteorsList;
    /// Random RotateVector;
    private float xV;
    private float yV;
    private float zV;
    private float speedRotate;
    
    
    /// <summary>
    ///  Ініціалізація параметрів метеориту.
    /// </summary>
    /// <param name="speed">скорість метеориту </param>
    /// <param name="damag"> урон метеориту</param>
    /// <param name="hp"> живучість метеориту</param>
    /// <param name="earthOb"> об'єкт "Earth"</param>
    /// <param name="meteors"> список з метеоритами щоб викреслити себе</param>
    public void Init(GameObject earthOb, List<GameObject> meteors, PointsCollector points, float rangeTargetPos)
    {
        maxHP = hP;
        earthObj = earthOb;

        // Розброс координат таргета метеоритів
        var tr = earthOb.transform.position;
        targetMove = new Vector3(tr.x + rangeTargetPos, tr.y + rangeTargetPos, tr.z);

        earthC = earthOb.GetComponent<Earth>();
        meteorsList = meteors;
        pointsCollector = points;
        // знижуєм щом не стерчав в планеті
        Destroy(gameObject, 35);

       xV = Random.Range(0, 360);
       yV = Random.Range(0, 360);
       zV = Random.Range(0, 360);
       speedRotate = Random.Range(0, 0.3f);
}

    private void OnTriggerEnter(Collider other)// реакція падіня метеориту.
    {
        if (other.gameObject == earthObj)
        {
            //Debug.Log("Метеорит впав на землю!");
            FX.Instance.DamagePlayerVibro();
            Earth.Instance.SetDamage(damage);
        }

        if (other.CompareTag("Rocket"))
        {
            //Debug.Log("Ракета попала в мееорит!");
            FX.Instance.DamageEnemyVibro();
            var touchRocket = other.gameObject.GetComponent<Rocket>();
            touchRocket.TouchMeteor();
           
            var points = (maxHP * distanceToEarh) / 10;

            // Відправили індекс ракети на обрахування множення очків
            MultiPoints.Instance.CheckIndexSequence(touchRocket.GetIndex());
            // Відправили очки за знищений метеорит
            pointsCollector.AddPoints((int)points, transform);

            Damage(touchRocket.GetDamageMeteor());
        }

        if (other.CompareTag("Earth"))
        {
            CreateParticeBoom();
            DestoyMeteor();
        }

    }

    public void Damage(float d)
    {
        hP -= d;
        // добавили загальний урон      
        LvlStatistic.Instance.AddStats(5, LvlStatistic.Instance.GetValuStats(5) + (int)d);

        if (hP <= 0)
        {
            meteorsList.Remove(gameObject);
            DestoyMeteor();
        }
    }

    private void DestoyMeteor()
    {
        //Debug.Log("Метеорит знищенно");   
        Destroy(gameObject);   
    }

    private void CreateParticeBoom()
    {
        Instantiate(particlesBoom, transform.position, transform.rotation);
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetMove, speedMove * Time.fixedDeltaTime);
        distanceToEarh = Vector3.Distance(transform.position, targetMove);

        transform.Rotate(new Vector3(xV, yV, zV) * Time.deltaTime * speedRotate);
    }

    private void OnDestroy()
    {
        LvlController.Instance.ChangeCountMeteors(-1);
    }

    public float GetHP()
    {
        return hP;
    }
    public float GetMaxHP()
    {
        return maxHP;
    }

    public float GetDist()
    {
        return distanceToEarh;
    }
}
