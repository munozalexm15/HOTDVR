using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandData : MonoBehaviour
{
    public enum HandModelType { LEFT, RIGHT };

    public HandModelType modelType;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}
