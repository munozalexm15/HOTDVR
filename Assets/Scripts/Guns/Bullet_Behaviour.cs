using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy_Damageable>() != null)
        {
            Debug.Log(collision.gameObject.name);
            collision.gameObject.GetComponent<Enemy_Damageable>().Health  -= 1;
            Destroy(gameObject);

        }
    }
}
