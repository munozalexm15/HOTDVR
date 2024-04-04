using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class LockPick_Beahavior : MonoBehaviour
{

    public GameObject lockpick;
    private bool isLockpickPulled = false;
    private Transform initialTransform;

    // Start is called before the first frame update
    void Start()
    {
        initialTransform = transform;
        //lockpick.GetComponent<XRGrabInteractable>().enabled = false;
        
       
        lockpick.GetComponent<XRGrabInteractable>().selectEntered.AddListener(checkRingIsPulled);
        lockpick.GetComponent<XRGrabInteractable>().selectExited.AddListener(restoreLockpickPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLockpickPulled)
        {
            transform.rotation = initialTransform.rotation;
            transform.position = initialTransform.position;
        }
    }

    private void CheckHand(SelectEnterEventArgs args)
    {
        HandData handData = args.interactorObject.transform.gameObject.GetComponentInChildren<HandData>();

        lockpick.SetActive(true);    }



    private void checkRingIsPulled(SelectEnterEventArgs args)
    {
        isLockpickPulled = true;

    }

    private void restoreLockpickPosition(SelectExitEventArgs args)
    {
        transform.rotation = initialTransform.rotation;
        transform.position = initialTransform.position;
        isLockpickPulled = false;
        
    }

}
