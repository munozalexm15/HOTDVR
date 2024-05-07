using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabPose_Handler : MonoBehaviour
{
    public HandData rightHandPose;
    public HandData leftHandPose;

    public float handPoseTransitionDuration = 0.2f;

    private Vector3 startingHandPosition;
    private Vector3 endingHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion endingHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[]  endingFingerRotations;
    
    [HideInInspector]
    public string actualHand = "";

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            //get the hand and freeze it so it doesnt move
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            if (handData.modelType == HandData.HandModelType.RIGHT)
            {
                SetHandDataValues(handData, rightHandPose);
                actualHand = "RIGHT";

            }
            else
            {
                actualHand = "LEFT";
                SetHandDataValues(handData, leftHandPose);
            }

            AudioSource handAudio = args.interactorObject.transform.GetComponentInChildren<AudioSource>();
            handAudio.Play();

            StartCoroutine( setHandDataCoRoutine(handData, endingHandPosition, endingHandRotation, endingFingerRotations, startingHandPosition, startingHandRotation, startingFingerRotations));
        }
    }


    public void UnSetPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            StartCoroutine(setHandDataCoRoutine(handData, startingHandPosition, startingHandRotation, startingFingerRotations, endingHandPosition, endingHandRotation, endingFingerRotations));
        }
           
    }

    //Cogemos las posiciones de los huesos de la mano principal (VR) y las pasamos a la del arma.
    //IMPORTANTE MANTENER EL ORDEN DE LOS HUESOS EN EL QUE SE AÑADEN A LA LISTA DE HANDDATA
    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startingHandPosition = new Vector3(h1.root.localPosition.x * h1.root.localScale.x, h1.root.localPosition.y * h1.root.localScale.y, h1.root.localPosition.z * h1.root.localScale.z);
        endingHandPosition = new Vector3(h2.root.localPosition.x * h2.root.localScale.x, h2.root.localPosition.y * h2.root.localScale.y, h2.root.localPosition.z * h2.root.localScale.z);

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

    //SMOOTH TRANSITION, MOVING BONES FRAME BY FRAME
    public IEnumerator setHandDataCoRoutine(HandData h, Vector3 newPos, Quaternion newRotation, Quaternion[] newBonesRotation, Vector3 startingPos, Quaternion startingRotation, Quaternion[] startingBonesRotation)
    {
        float timer = 0;


        while (timer < handPoseTransitionDuration)
        {
            Vector3 p = Vector3.Lerp(startingPos, newPos, timer / handPoseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / handPoseTransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;

            for (int i = 0; i < newBonesRotation.Length; i++)
            {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / handPoseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

#if UNITY_EDITOR

    [MenuItem("Tools/Mirror selected right hand pose")]
    public static void MirrorRightPose()
    {
        Debug.Log("MIRROR RIGHT POSE");
        GrabPose_Handler handPose  = Selection.activeGameObject.GetComponent<GrabPose_Handler>();
        handPose.MirrorPose(handPose.leftHandPose, handPose.rightHandPose);
    }

#endif

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
    {
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;

        Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
        mirroredQuaternion.y *= -1;
        mirroredQuaternion.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredQuaternion;

        for (int i =0; i < poseUsedToMirror.fingerBones.Length; i++)
        {
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
        }
    }
}
