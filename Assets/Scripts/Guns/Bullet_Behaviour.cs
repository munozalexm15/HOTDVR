using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{

    public float bulletDamage;


    private void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform topLevelParent = FindTopLevelParent(collision.gameObject.transform);
        if (topLevelParent.gameObject.GetComponent<EnemyAI>() != null)
        {
            if (topLevelParent.gameObject.GetComponent<EnemyAI>().enemyDamagedStatus == null)
            {
                topLevelParent.gameObject.GetComponent<EnemyAI>().enemyDamagedStatus = topLevelParent.gameObject.GetComponent<EnemyAI>().StartCoroutine("FlashDamageZombie");
            }
            topLevelParent.gameObject.GetComponent<EnemyAI>().EnemyHealth -= bulletDamage;
            Destroy(gameObject);
        }

    }

    private Transform FindTopLevelParent(Transform child)
    {
        Transform parent = child.parent;

        while (parent != null)
        {
            child = parent;
            parent = child.parent;
        }
        return child;
    }
}