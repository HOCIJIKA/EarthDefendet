using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteArrow : MonoBehaviour
{
    private Satellite satellite;
    private Vector3 start;
    private Vector3 direction;
    private RaycastHit hit;

    private void Start()
    {
        satellite = FindObjectOfType<Satellite>();
    }

    private void FixedUpdate()
    {
        start = transform.TransformPoint(transform.localPosition);
        direction = transform.TransformDirection(Vector3.up);
        //RaycastHit hit;
        Debug.DrawRay(start, direction * 5, Color.red);

        if (Physics.Raycast(start, direction, out hit))
        {
            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), hit.transform.GetComponent<Collider>().bounds))
            {
                if (hit.transform.CompareTag("Meteor"))
                {
                    satellite.DetectMeteor(true);
                }
                else
                    satellite.DetectMeteor(false);
            }
            else
                satellite.DetectMeteor(false);
        }
        else
            satellite.DetectMeteor(false);
    }
}
