using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade_Behaviour : XRGrabInteractable
{

    public GameObject ring;
    // Start is called before the first frame update
    void Start()
    {
        ring.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        selectEntered.AddListener(CheckHand);
    }

    private void CheckHand(SelectEnterEventArgs args)
    {
        HandData handData = args.interactorObject.transform.gameObject.GetComponentInChildren<HandData>();
        if (handData.modelType == HandData.HandModelType.LEFT )
        {
            ring.transform.Rotate(new Vector3(0, 360, 0));
        }
        else if (handData.modelType == HandData.HandModelType.RIGHT)
        {
            ring.transform.Rotate(new Vector3(0, 180, 0));
        }

    }

}
