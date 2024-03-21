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

    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float Distance = Vector3.Distance(Player.position, Enemy.position);
        Debug.Log("Distancia: " + Distance);


        if (Distance < 1.25f)
        {
            Debug.Log("Enemigo ataca a Melee");
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
        }
        else 
        {
            EnemyNav.SetDestination(Player.position);
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }
    }

}
