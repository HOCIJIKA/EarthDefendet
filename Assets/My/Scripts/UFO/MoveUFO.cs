using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUFO : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Life value")]
    [SerializeField] private float hP; // життя метеориту
    [Header("Move value")]
    [SerializeField] private float circleDist; // distans to start move on circle
    [SerializeField] private float angle = 0; 
    [SerializeField] private float radius = 0.5f; 
    [SerializeField] private float speed = 1f;
#pragma warning restore 0649

    private Vector2 targetPos;
    private float x, y, tX, tY, points, maxHP;
    private float startRadius;
    private PointsCollector pointsCollector;
    private List<GameObject> meteorsList;
    private Rocket touchRocket;

    public void Init(PointsCollector p, Earth e, List<GameObject> meteors, float sr)
    {
        maxHP = hP;
        targetPos = e.transform.position;
        pointsCollector = p;
        meteorsList = meteors;

        tX = targetPos.x;
        tY = targetPos.y;
        angle = Random.Range(0, 12);
        startRadius = sr;
    }

    void Update()
    {
        if (radius < startRadius)
        {
            startRadius -= Time.deltaTime / 3;
        }
        angle += Time.deltaTime;
        x = Mathf.Cos(angle * speed) * startRadius;
        y = Mathf.Sin(angle * speed) * startRadius;
        transform.position = new Vector2(x, y) + new Vector2(tX, tY);

        transform.LookAt(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket"))
        {
            //Debug.Log("Ракета попала в мееорит!");
            FX.Instance.DamageEnemyVibro();
            touchRocket = other.gameObject.GetComponent<Rocket>();
            touchRocket.TouchMeteor();
            points = (maxHP * startRadius) / 10;
            // Відправили індекс ракети на обрахування множення очків
            MultiPoints.Instance.CheckIndexSequence(touchRocket.GetIndex());
            // Відправили очки за знищений метеорит
            pointsCollector.AddPoints((int)points, transform);

            Damage(touchRocket.GetDamageMeteor());
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
            DestoyUFO();
        }
    }
    private void DestoyUFO()
    {
        //Debug.Log("Метеорит знищенно");   
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        LvlController.Instance.ChangeCountMeteors(-1);
    }
}
