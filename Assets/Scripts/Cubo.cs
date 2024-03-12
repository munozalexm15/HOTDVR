using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubo : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        UnityEngine.Debug.Log("Me dio: " + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Me dio por trigger: " + other.gameObject.name);
    }
}
