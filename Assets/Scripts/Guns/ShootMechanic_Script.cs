using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public Material reloadMaterial;

    public Material gunMaterial;


    public GameObject[] gunParts;

    private string actualHand;

    public TMP_Text weaponAmmoIndicator;
    void Start()
    {
        
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
        grabbable.selectExited.AddListener(HideAmmo);
        grabbable.selectEntered.AddListener(ShowAmmo);

        gunData = GetComponent<GunData>();
        actualHand = GetComponent<GrabPose_Handler>().actualHand;

    }

 
    public void FireBullet(ActivateEventArgs args)
    {
        if (gunData.isReloading)
        {
            return;
        }

        if (gunData.bulletsInMagazine <= 0)
        {
            StartCoroutine(reloadWeapon());
            return;
        }
        else
        {
            weaponAmmoIndicator.SetText(gunData.bulletsInMagazine + " / " + gunData.bulletsPerMagazine);
        }

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
        gunData.bulletsInMagazine -= 1;

        StartCoroutine(checkBulletStatus(spawnedBullet));
    }

    public IEnumerator reloadWeapon()
    {

        float lerp = Mathf.PingPong(Time.time, gunData.reloadTime) / gunData.reloadTime;
        gunData.isReloading = true;
        weaponAmmoIndicator.SetText("Rel");

        //GetComponent<Renderer>().material.Lerp(gunMaterial, reloadMaterial, lerp);
        GetComponent<Renderer>().material = reloadMaterial;

        for (int i = 0; i < gunParts.Length; i++)
        {
            gunParts[i].SetActive(false);
        }
        //before reload
        yield return new WaitForSeconds(gunData.reloadTime);

        //after reload
        //GetComponent<Renderer>().material.Lerp(reloadMaterial, gunMaterial, lerp);
        GetComponent<Renderer>().material = gunMaterial;
        
        for (int i = 0; i < gunParts.Length; i++)
        {
            gunParts[i].SetActive(true);
        }

        gunData.bulletsInMagazine = gunData.bulletsPerMagazine;
        weaponAmmoIndicator.SetText(gunData.bulletsInMagazine + "  / " + gunData.bulletsPerMagazine);
        gunData.isReloading = false;
    }

    private void ShowAmmo(SelectEnterEventArgs arg0)
    {
        weaponAmmoIndicator.SetText(gunData.bulletsInMagazine + " / " + gunData.bulletsPerMagazine);
        if (gunData.bulletsInMagazine <= 0)
        {
            StartCoroutine(reloadWeapon());
        }

    }


    private void HideAmmo(SelectExitEventArgs arg0)
    {
        weaponAmmoIndicator.text = "";
    }

    public IEnumerator checkBulletStatus(GameObject spawnedBullet)
    {
        yield return new WaitForSeconds(2);

        if (!spawnedBullet.IsDestroyed())
        {
            Destroy(spawnedBullet);
        }

    }
}
