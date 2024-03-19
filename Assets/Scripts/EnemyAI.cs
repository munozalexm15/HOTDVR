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
        EnemyNav.SetDestination(Player.position);
        animator.SetBool("Walk", true);

        float Distance = Vector3.Distance(Player.position, Enemy.position);

        if (Distance < 1f)
        {
            Debug.Log("Enemigo ataca a Melee");
        }
    }

}
