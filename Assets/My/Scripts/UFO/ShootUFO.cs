using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootUFO : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Shot Options")]
    [SerializeField] private float speedShot;
    [SerializeField] private float delayShot;
    [SerializeField] private Transform spamPos;
    [Header("Shot Prefabe")]
    [SerializeField] private GameObject shotPref;
    [SerializeField] private Transform shootsUFOContainer;

#pragma warning restore 0649


    private void Start()
    {
        StartCoroutine(DealyCreateShot());
    }

    private void CreateShot()
    {
        // shooting;
        var s = Instantiate(shotPref, spamPos.position, spamPos.rotation, shootsUFOContainer);
    }

    private IEnumerator DealyCreateShot()
    {
        yield return new WaitForSeconds(Random.Range(delayShot, delayShot*3));
        CreateShot();
        StartCoroutine(DealyCreateShot());
    }
}
