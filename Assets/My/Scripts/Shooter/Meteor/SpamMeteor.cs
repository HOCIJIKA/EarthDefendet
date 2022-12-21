
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamMeteor : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private PointsCollector pointsCollector;
    [Space(10)]
    [Header("Настройки спамера")]
    [SerializeField, Range(1, 1.5f)] private float multRandomScale; 
    [SerializeField] private Transform spamPos;
    [SerializeField] private Transform meteorsContainer;
    [SerializeField] private float delaySpam;
    [Header("Настройки обертання")]
    [SerializeField] private float angle;
    [SerializeField] private float radius;
    [SerializeField] private float speed;
    [Header("Всі воожі елементи")]
    [Tooltip("0 - найпростіший елемент")]
    [SerializeField] private List<GameObject> AllEnemies;
    [Header("Настройки Ворогів")]
    [SerializeField] private GameObject meteor;
    [SerializeField] private float rangeTargetPos;
    [SerializeField] private float speedMoveMeteor; //скорість руху
    [SerializeField] private int damage; // урон метеориту
    [SerializeField] private float hP; // життя метеориту
    [SerializeField] private GameObject earthObj;
    [Space(10)]
    [SerializeField] private List<GameObject> meteors;
    [SerializeField] private List<int> indexLvlEnemies;// по суті не потрібно в редакторі.Серіалізовано для моніторингу.
#pragma warning restore 0649

    private Vector2 targetPos;
    private float x, y;
    private float tX, tY;
    private int maxCountMeteors;

    private static SpamMeteor instance;
    public static SpamMeteor Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init(int mcm)
    {
        maxCountMeteors = mcm;

        CreateListEnemies();
        //SpamMeteoryt();
        StartCoroutine(DelaySpam());

        targetPos = FindObjectOfType<Earth>().transform.position;
        tX = targetPos.x;
        tY = targetPos.y;
        //angle = Random.Range(0, 12);
    }

    /// <summary>
    /// Ррозрахунок спіска ворогів для поточного рівня.
    /// </summary>
    private void CreateListEnemies()
    {
        var curentLvl = GameC.Instance.GetData().CurrentLvl;
        var listOnLvl = new List<int>();
        var proh = curentLvl; 
        var n = 0;

        if (curentLvl > n)
            n = curentLvl;

        //Ініціалізація масива.
        for (int i = 0; i < 200; i++)
            listOnLvl.Add(0);

        //заповнюєм масиви в масиві
        for (int i = 0; i < listOnLvl.Count; i++)
        {
            proh--;

            if (n < 0)
                n = 0;

            if (n > AllEnemies.Count -1)
                n = AllEnemies.Count -1;

            for (int y = 0; y < n + 1; y++)
                listOnLvl[i] = y;             

            if (listOnLvl[i] == n && proh < n)
            {
                n--;
                proh = curentLvl;
            }
        }
        // записали результат підбору.
        indexLvlEnemies = listOnLvl;
    }

    private void Update()
    {
        // Move on circle
        angle += Time.deltaTime;
        x = Mathf.Cos(angle * speed) * radius;
        y = Mathf.Sin(angle * speed) * radius;
        transform.position = new Vector2(x, y) + new Vector2(tX, tY);       
    }

    IEnumerator DelaySpam()
    {
        yield return new WaitForSeconds(Random.Range(delaySpam * 0.8f, delaySpam * 1.2f));
        SpamEnemy();

        if (maxCountMeteors > 0)
            StartCoroutine(DelaySpam());
    }

    private void SpamEnemy()
    {
        maxCountMeteors--;
        var i = indexLvlEnemies[Random.Range(0, indexLvlEnemies.Count)];
        var m = AllEnemies[i];
        var curentEnemy = Instantiate(m, spamPos.position, spamPos.rotation, meteorsContainer);
        curentEnemy.tag = "Meteor";

        if (!curentEnemy.GetComponent<MoveUFO>())
        {
            var v = curentEnemy.transform.localScale;
            curentEnemy.transform.localScale = new Vector3(v.x * Random.Range(1, multRandomScale), v.y * +Random.Range(1, multRandomScale), v.z * Random.Range(1, multRandomScale));
        }

        var rand = Random.Range(-rangeTargetPos, rangeTargetPos);

        if (curentEnemy.GetComponent<Meteor>())
            curentEnemy.GetComponent<Meteor>().Init(earthObj, meteors, pointsCollector, rand);
        else if (curentEnemy.GetComponent<MoveUFO>())
            curentEnemy.GetComponent<MoveUFO>().Init(pointsCollector, earthObj.GetComponent<Earth>(), meteors, radius);
       
        meteors.Add(curentEnemy);
        LvlController.Instance.ChangeCountMeteors(1);
    }
}
