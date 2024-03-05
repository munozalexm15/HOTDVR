using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://discussions.unity.com/t/casting-ray-forward-from-transform-position/48120/2

public class Crosshair_Mechanic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                Debug.DrawLine(transform.position, hit.transform.position, Color.red);
            }
        }
    }
}
