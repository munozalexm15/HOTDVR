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

    //Not the hands in the prefab, but the ones being controlled in the main XR locomotion system
    public HandData rightHandPose_XR;
    public HandData leftHandPose_XR;

    private string actualHand;
    void Start()
    {
        
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);

        gunData = GetComponent<GunData>();
        actualHand = GetComponent<GrabPose_Handler>().actualHand;
    }

    public void FireBullet(ActivateEventArgs args)
    {
        actualHand = GetComponent<GrabPose_Handler>().actualHand;
        //Actually made for a semi-auto weapon, it might change later if we implement full auto weapons

        GameObject spawnedBullet = Instantiate(bulletModel);
        spawnedBullet.transform.position = spawnPoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * gunData.bulletSpeed;

        if (actualHand.Equals("LEFT"))
        {
            leftHandPose_XR.GetComponent<Animation>().Play(gunData.LeftHandTriggerPull);
        }

        else if (actualHand.Equals("RIGHT"))
        {
            rightHandPose_XR.GetComponent<Animation>().Play(gunData.RightHandTriggerPull);

        }

        gunAnimations.PlayQueued(gunData.EjectAnimation);


        BulletEjectParticle.Emit(1);
        muzzleFlashParticle.Emit(1);
        Destroy(spawnedBullet, 3);



    }
}
