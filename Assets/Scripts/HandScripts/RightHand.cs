using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RightHand : MonoBehaviour
{
    private InputData _inputData;
    void Start()
    {
        UnityEngine.Debug.Log("Inicio el input data derecho");
        _inputData = GetComponent<InputData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Objeto tocado derecha: " + other.gameObject.name);
        _inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        if (other.gameObject.tag.Equals("Enemy"))
        {
            if (_inputData._rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger) && _inputData._rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool grip))
            {

                if (trigger && grip)
                {
                    //other.attachedRigidbody.AddForce(transform.forward * (velocity.magnitude * 50f), ForceMode.Impulse);
                    //UnityEngine.Debug.Log("Fuerza aplicada puño al derecho: " + velocity.magnitude);
                    if (velocity.magnitude > 0.5)
                    {
                        other.gameObject.GetComponent<Animator>().SetTrigger("IsPunched");
                        other.gameObject.GetComponent<EnemyAI>().EnemyHealth -= 1;
                        UnityEngine.Debug.Log(other.gameObject.GetComponent<EnemyAI>().EnemyHealth);
                    }
                }
                else
                {
                    //other.attachedRigidbody.AddForce(transform.forward * (velocity.magnitude * 25f), ForceMode.Impulse);
                    //UnityEngine.Debug.Log("Fuerza aplicada al empujar a la derecha: " + velocity.magnitude);
                    if (velocity.magnitude > 0.5)
                    {
                        other.gameObject.GetComponent<Animator>().SetTrigger("IsHit");
                    }
                }

            }
        }
    }
}
