using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damageable : MonoBehaviour
{

    public float Health = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Health > 0)
        {
            Destroy(gameObject);
        }
    }
}
