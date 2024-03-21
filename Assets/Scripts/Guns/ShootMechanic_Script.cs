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

    public Transform crosshairTransform;

    public ActionBasedController LeftController;
    public ActionBasedController RightController;

    private Coroutine _current;

    private float bulletSpread;

    ActivateEventArgs activateEventArgs;

    private bool isShooting;

    private float lastfired;

    void Start()
    {
        bulletSpread = 0;
        isShooting = false;
        gunData = GetComponent<GunData>();
        actualHand = GetComponent<GrabPose_Handler>().actualHand;
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        
        if (!gunData.isAutomatic)
        {
            grabbable.activated.AddListener(FireBullet);
        }
        else
        {
            grabbable.activated.AddListener(FullAutoShoot);
            grabbable.deactivated.AddListener(StopShooting);

        }
      
        grabbable.selectExited.AddListener(HideAmmo);
        grabbable.selectEntered.AddListener(ShowAmmo);

    }

    void Update()
    {
        Crosshair_ResetSize();

        if (isShooting)
        {
            if (Time.time - lastfired > 1 / gunData.firerate)
            {
                lastfired = Time.time;
                FireBullet(activateEventArgs);
            }
        }
    }

    public void StopShooting(DeactivateEventArgs args)
    {
        isShooting = false;
    }

    public void FullAutoShoot(ActivateEventArgs args)
    {
        isShooting = true;
        activateEventArgs = args;
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

        actualHand = GetComponent<GrabPose_Handler>().actualHand;

        //BUG: Mirar tema de la rotacion del arma o de la mano para que dependa de su rotacion local --- Manual Reload (Force player to put the gun in an almost 90 degree angle and press shoot to reload)
        if (actualHand == "LEFT" && LeftController.transform.localRotation.x <= -0.7)
        {
           
            StartCoroutine(reloadWeapon());
            return;
        }
        else if (actualHand == "RIGHT" && RightController.transform.localRotation.x <= -0.7)
        {
            StartCoroutine(reloadWeapon());
            return;
        }


            //Actually made for a semi-auto weapon, it might change later if we implement full auto weapons

        GameObject spawnedBullet = Instantiate(bulletModel);
        spawnedBullet.transform.position = spawnPoint.position;
        //Add bullet spread
        spawnedBullet.transform.Rotate(UnityEngine.Random.Range(0, bulletSpread), UnityEngine.Random.Range(0, bulletSpread), 0);
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

        Crosshair_MakeBigger();

        weaponAmmoIndicator.SetText(gunData.bulletsInMagazine + " / " + gunData.bulletsPerMagazine);

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
        crosshairTransform.gameObject.SetActive(true);
    }


    private void HideAmmo(SelectExitEventArgs arg0)
    {
        weaponAmmoIndicator.text = "";
        crosshairTransform.gameObject.SetActive(false);
    }

    public IEnumerator checkBulletStatus(GameObject spawnedBullet)
    {
        yield return new WaitForSeconds(2);

        if (!spawnedBullet.IsDestroyed())
        {
            Destroy(spawnedBullet);
        }
    }

    public void Crosshair_MakeBigger()
    {
        crosshairTransform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        bulletSpread += 10f;
    }

    public void Crosshair_ResetSize()
    {
        if (crosshairTransform.localScale.x > 0.2)
        {
            crosshairTransform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            bulletSpread -= 0.01f;
        }
    }
}
