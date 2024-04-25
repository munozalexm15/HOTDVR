using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform topLevelParent = FindTopLevelParent(collision.gameObject.transform);
        if (topLevelParent.gameObject.GetComponent<EnemyAI>() != null)
        {
            topLevelParent.gameObject.GetComponent<EnemyAI>().EnemyHealth -= 1;
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