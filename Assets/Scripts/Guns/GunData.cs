using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData : MonoBehaviour
{

    public float firerate;
    public float bulletSpeed;
    public int bulletsPerMagazine;
    public int bulletsInMagazine;
    public float reloadTime;
    public float accuracy;

    public bool isReloading;

    public float damage;

    public string EjectAnimation;
    public string RightHandTriggerPull;
    public string LeftHandTriggerPull;
}
