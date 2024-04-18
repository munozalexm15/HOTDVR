using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy_Damageable>() != null)
        {
            collision.gameObject.GetComponent<Enemy_Damageable>().Damaged();
            
            Destroy(gameObject);

        }
    }
}
