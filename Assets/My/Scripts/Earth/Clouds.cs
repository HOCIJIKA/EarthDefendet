using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Скорість по Х")]
    [SerializeField] private float speedX;
    [Header("Скорість по Y")]
    [SerializeField] private float speedY;
#pragma warning restore 0649


    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, speedY, 0));
        transform.Rotate(new Vector3(speedX, 0, 0));
    }
}
