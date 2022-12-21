using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShot : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float speed;
    [SerializeField] private float rangeTargetPos;
#pragma warning restore 0649

    private Vector2 target;

    private void Start()
    {
        target = FindObjectOfType<Earth>().transform.position;
        target = new Vector2(target.x + rangeTargetPos, target.y + rangeTargetPos);
        transform.LookAt(target);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
    }
}
