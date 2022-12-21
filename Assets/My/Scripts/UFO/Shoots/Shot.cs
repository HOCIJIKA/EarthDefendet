using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private int damage;
    [SerializeField] private GameObject particlesBoom;
#pragma warning restore 0649

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Earth"))
        {
            Earth.Instance.SetDamage(damage);
            CreateParticeBoom();
            DestoyMeteor();
        }
        if (other.CompareTag("Shield"))
        {
            other.GetComponent<Shield>().TakeDamage();
            CreateParticeBoom();
            DestoyMeteor();
        }
    }

    private void DestoyMeteor()
    {
        //Debug.Log("Shot of UFO");   
        Destroy(gameObject.transform.parent.gameObject);
    }
    private void CreateParticeBoom()
    {
        Instantiate(particlesBoom, transform.position, transform.rotation);
    }
}
