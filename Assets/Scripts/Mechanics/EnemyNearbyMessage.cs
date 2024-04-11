using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyNearbyMessage : MonoBehaviour
{

    public TMP_Text m_TextMeshPro;
    public GameObject player;
    public Transform enemyDistance;
    //Pillar collider, hacerle un ontriggerenter
    //Coger el gameObject (si es zombie) y mirar la distancia que hay entre el zombie y la camara / personaje
    //Ir cambiando el alpha en base a la distancia

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (enemyDistance != null)
        {
            print(1.0f / Vector3.Distance(player.transform.position, enemyDistance.position));
            m_TextMeshPro.alpha = 1.0f / Vector3.Distance(player.transform.position, enemyDistance.position);
        }
      else
        {
            m_TextMeshPro.alpha = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyDistance = other.gameObject.transform;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        enemyDistance = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyDistance = other.gameObject.transform;
        }
    }
}
