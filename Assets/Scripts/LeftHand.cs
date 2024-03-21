using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeftHand : MonoBehaviour
{
    private InputData _inputData;
    void Start()
    {
        UnityEngine.Debug.Log("Inicio el input data izquierdo");
        _inputData = GetComponent<InputData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Objeto tocado izquierda: " + other.gameObject.name);
        _inputData._leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        if(other.gameObject.name.Equals("Zombie")) { 
            if (_inputData._leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger) && _inputData._leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool grip))
            {
            
                if (trigger && grip)
                {
                    other.attachedRigidbody.AddForce(transform.forward * (velocity.magnitude * 50f), ForceMode.Impulse);
                    UnityEngine.Debug.Log("Fuerza aplicada puño izquierdo: " + velocity.magnitude);
                }
                else
                {
                    other.attachedRigidbody.AddForce(transform.forward * (velocity.magnitude * 25f), ForceMode.Impulse);
                    UnityEngine.Debug.Log("Fuerza aplicada empujar izquierdo: " + velocity.magnitude);
                }

            }
        }

    }
}
