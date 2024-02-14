using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPose_Handler : MonoBehaviour
{
    public HandData rightHandPose;

    private Vector3 startingHandPosition;
    private Vector3 endingHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion endingHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[]  endingFingerRotations;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);

        rightHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            //get the hand and freeze it so it doesnt move
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            SetHandDataValues(handData, rightHandPose);

            SetHandData(handData, endingHandPosition, endingHandRotation, endingFingerRotations);
        }
    }

    //Cogemos las posiciones de los huesos de la mano principal (VR) y las pasamos a la del arma.
    //IMPORTANTE MANTENER EL ORDEN DE LOS HUESOS EN EL QUE SE AÑADEN A LA LISTA DE HANDDATA
    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startingHandPosition = h1.root.localPosition;
        endingHandPosition = h2.root.localPosition;

        startingHandRotation = h1.root.localRotation;
        endingHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        endingFingerRotations = new Quaternion[h1.fingerBones.Length];

        for (int i =0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            endingFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPos, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPos;
        h.root.localRotation = newRotation;

        for (int i = 0; i <  newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }
}
