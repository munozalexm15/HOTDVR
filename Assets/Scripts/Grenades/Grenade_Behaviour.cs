using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade_Behaviour : XRGrabInteractable
{

    public GameObject ring;
    private Transform ringInitialPos;
    private bool IsRingPulled = false;
    private Transform initialTransform;
    public ParticleSystem explosionParticles;
    public PathTracking_Behaviour player_Data;

    //For the explosion
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;

    // Start is called before the first frame update
    void Start()
    {
        ringInitialPos = ring.GetComponent<Transform>();
        ring.SetActive(false);
        initialTransform = transform;
        //lockpick.GetComponent<XRGrabInteractable>().enabled = false;
        selectEntered.AddListener(CheckHand);
        selectExited.AddListener(HideRing);
        ring.GetComponent<XRGrabInteractable>().selectEntered.AddListener(checkRingIsPulled);
        ring.GetComponent<XRGrabInteractable>().selectExited.AddListener(addGravityToRing);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsRingPulled)
        {
            ring.transform.position = transform.position;
        }
        explosionParticles.transform.position = transform.position;


    }

    private void CheckHand(SelectEnterEventArgs args)
    {
        HandData handData = args.interactorObject.transform.gameObject.GetComponentInChildren<HandData>();

        /**
        if (handData.modelType == HandData.HandModelType.LEFT)
        {
            lockpick.transform.Rotate(new Vector3(0, 360, 0));
        }

        else if (handData.modelType == HandData.HandModelType.RIGHT)
        {
            lockpick.transform.Rotate(new Vector3(0, 180, 0));
        }
        **/

        ring.SetActive(true);
        //lockpick.gameObject.GetComponent<XRGrabInteractable>().enabled = true;
    }

    private void HideRing(SelectExitEventArgs args)
    {
        ring.SetActive(false);

    }

    private void checkRingIsPulled(SelectEnterEventArgs args)
    {
        IsRingPulled = true;
        ring.transform.parent = null;
        StartCoroutine(destroyRingExplodeNade());
    }

    private void addGravityToRing(SelectExitEventArgs args)
    {
        transform.rotation = initialTransform.rotation;
        if (ring.transform.position != transform.position)
        {
            ring.GetComponent<Rigidbody>().useGravity = true;
        }
        
    }

    public IEnumerator destroyRingExplodeNade()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
        explosionParticles.Play();
        ExplodeNonAlloc();
    }

    void ExplodeNonAlloc()
    {
        Collider[] collList = Physics.OverlapSphere(transform.position, explosionRadius);

        if (collList.Length > 0)
        {
            for (int i = 0; i < collList.Length; i++)
            {
                if (collList[i].tag == "Enemy") {

                    collList[i].gameObject.GetComponent<EnemyAI>().EnemyHealth = 0;
                    Destroy(collList[i]);
                    player_Data.kills += 1;
                }
                if (collList[i].TryGetComponent(out Rigidbody rb))
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 3);
                }
            }
        }
        Destroy(ring);
        Destroy(gameObject);
    }

}
