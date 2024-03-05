using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://discussions.unity.com/t/casting-ray-forward-from-transform-position/48120/2

public class Crosshair_Mechanic : MonoBehaviour
{
    RaycastHit crosshairRay;

    [SerializeField] GameObject crosshair;

    void Start()
    {
        
    }


    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData, 6))
        {

            if (hitData.transform.gameObject.layer == 7)
            {
                return;
            }

            crosshair.transform.position = new Vector3(hitData.point.x, hitData.point.y, hitData.point.z);

            //If layer is "Behind Crosshair" move it so it stays on top of the gameObject
            if (hitData.transform.gameObject.layer == 6)
            {
                crosshair.transform.position = new Vector3(hitData.point.x -0.1f, hitData.point.y, hitData.point.z - 0.1f);
            }
           
        }
    }
}
