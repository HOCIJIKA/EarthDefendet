using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Shooter shooter;
    [SerializeField] private Transform startPoint;
    [Space(20)]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color detectedlColor;
#pragma warning restore 0649

    private float distanceToShot;
    private SpriteRenderer spriteRenderer;
    private Autoplay autoplay;

    private Vector3 start;
    private Vector3 direction;
    private RaycastHit hit;
    private Ray ray;

    private static Arrow instance;
    public static Arrow Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            spriteRenderer.color = detectedlColor;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            spriteRenderer.color = normalColor;
        }
    }

    private void FixedUpdate()
    {
        if (autoplay != null && autoplay.gameObject.activeSelf)
        {
            start = startPoint.position;// (startPoint.localPosition);
            direction = transform.TransformDirection(Vector3.up * distanceToShot * 1.1f);
            //RaycastHit hit;
            Debug.DrawRay(start, direction, Color.red);

            ray = new Ray(start, direction);
            if (Physics.Raycast(ray, out hit, distanceToShot * 1.1f))
            {
                if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), hit.transform.GetComponent<Collider>().bounds))
                {
                    if (hit.transform.CompareTag("Meteor"))
                    {
                        autoplay.DetectMeteor(true);
                    }
                    else
                        autoplay.DetectMeteor(false);
                }
                else
                    autoplay.DetectMeteor(false);
            }
            else
                autoplay.DetectMeteor(false);
        }
    }

    public void SetDistandeToShot(float value)
    {
        distanceToShot = value;
    }

    public void SetAutoPlay(Autoplay a)
    {
        autoplay = a;
    }
}
