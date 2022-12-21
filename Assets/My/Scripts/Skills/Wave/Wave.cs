using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// покищо скейл контролюється анімацією. Пізніше переробити на скріпт 
/// і контролювати розмір з рівнем прокачки.
/// Також добавити урон хвилі а не знищення обьекта.
/// </summary>
public class Wave : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject exsplosion;
    [Header("система частинок")]
    [SerializeField] private float delayTime;
    [SerializeField] private GameObject particles;
#pragma warning restore 0649

    private float damage;

    public void Init(float d)
    {
        damage = d;
        Destroy(gameObject.transform.parent.gameObject,1);
        StartCoroutine(CreateParticle());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            var meteorC = other.GetComponent<Meteor>();
            var points = (meteorC.GetMaxHP() * meteorC.GetDist()) / 10;
            PointsCollector.Instance.AddPoints((int)points, other.transform);

            meteorC.Damage(damage);

            if (meteorC.GetHP() <= 0)
                Instantiate(exsplosion, other.transform.position, other.transform.rotation);
        }
    }

    private IEnumerator CreateParticle()
    {
        yield return new WaitForSeconds(delayTime);
        var p = Instantiate(particles, FindObjectOfType<Earth>().transform);
        p.transform.localScale = gameObject.transform.parent.localScale;
    }
}
