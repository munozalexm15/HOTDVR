using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damageable : MonoBehaviour
{

    public float Health;

    public Material zombieMaterial;

    public PathTracking_Behaviour PlayerPath;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            PlayerPath.kills += 1;
            Destroy(gameObject);
        }
    }

    public IEnumerator FlashDamage()
    {

        GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;

        yield return new WaitForSeconds(0.25f);

        GetComponentInChildren<SkinnedMeshRenderer>().material = zombieMaterial;
    }

    public void Damaged()
    {
        Health -= 1;

        StartCoroutine(FlashDamage());
    }
}
