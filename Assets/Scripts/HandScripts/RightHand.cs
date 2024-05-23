using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RightHand : MonoBehaviour
{
    private InputData _inputData;
    void Start()
    {
        _inputData = GetComponent<InputData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform topLevelParent = FindTopLevelParent(other.gameObject.transform);
        _inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        if (topLevelParent.gameObject.tag.Equals("Enemy"))
        {
            if (_inputData._rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger) && _inputData._rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool grip))
            {

                if (trigger && grip)
                {
                    if (velocity.magnitude > 0.5)
                    {
                        topLevelParent.gameObject.GetComponent<Animator>().SetTrigger("IsPunched");
                        topLevelParent.gameObject.GetComponent<EnemyAI>().EnemyHealth -= 1;
                        UnityEngine.Debug.Log(topLevelParent.gameObject.GetComponent<EnemyAI>().EnemyHealth);
                    }
                }
                else
                {
                    if (velocity.magnitude > 0.5)
                    {
                        topLevelParent.gameObject.GetComponent<Animator>().SetTrigger("IsHit");
                    }
                }

            }
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
