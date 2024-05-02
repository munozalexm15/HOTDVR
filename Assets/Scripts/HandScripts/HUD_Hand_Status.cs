using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Hand_Status : MonoBehaviour
{

    public Canvas hud_status;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localRotation.z <= -0.4)
        {
            hud_status.gameObject.SetActive(true);
        }
        else
        {
            hud_status.gameObject.SetActive(false);
        }
    }
}
