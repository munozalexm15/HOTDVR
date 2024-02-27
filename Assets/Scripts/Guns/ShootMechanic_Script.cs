using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static HandData;

public class ShootMechanic_Script : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bulletModel;

    public Transform spawnPoint;

    private GunData gunData;

    public Animation gunAnimations;

    public ParticleSystem BulletEjectParticle;

    public ParticleSystem muzzleFlashParticle;
    void Start()
    {
        
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);

        gunData = GetComponent<GunData>();
        //gunAnimations.AddClip(gunData.RightHandTriggerPull, "RightHandTriggerPull");
        //gunAnimations.AddClip(gunData.LeftHandTriggerPull, "LeftHandTriggerPull");
    }

    public void FireBullet(ActivateEventArgs args)
    {
        //Actually made for a semi-auto weapon, it might change later if we implement full auto weapons

        GameObject spawnedBullet = Instantiate(bulletModel);
        spawnedBullet.transform.position = spawnPoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * gunData.bulletSpeed;
        gunAnimations.Play(gunData.EjectAnimation);

        //find hand_data component in children and, depending of which hand is grabbing the gun, play one animation or another

       
        BulletEjectParticle.Emit(1);
        muzzleFlashParticle.Emit(1);
        Destroy(spawnedBullet, 3);



    }
}
