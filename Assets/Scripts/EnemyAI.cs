using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent EnemyNav;
    public Transform Player;
    public Transform Enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyNav.SetDestination(Player.position);

        float Distance = Vector3.Distance(Player.position, Enemy.position);

        if (Distance < 1f) 
        {
            Debug.Log("Enemigo ataca a Melee"); 
        }

    }

}
