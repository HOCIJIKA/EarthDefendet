using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject exsplosion;
    [SerializeField] private float delayTime;
    [Header("система частинок")]
    [SerializeField] private List<GameObject> particlesShield;
#pragma warning restore 0649

    private float hp;


    public void Init(float h)
    {
        hp = h;
        particlesShield[Random.Range(0, particlesShield.Count)].SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            var meteorC = other.GetComponent<Meteor>();
            var points = (meteorC.GetMaxHP() * meteorC.GetDist()) / 10;
            PointsCollector.Instance.AddPoints((int)points, other.transform);

            meteorC.Damage(meteorC.GetHP());
            Instantiate(exsplosion, other.transform.position, other.transform.rotation);

            TakeDamage();
        }
    }

    public void TakeDamage()
    {

        hp--;

        if (hp <= 0)
            DestroyShield();
    }

    private void DestroyShield()
    {
        Destroy(gameObject);
    }
}
