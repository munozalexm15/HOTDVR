using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent EnemyNav;
    public GameObject Player;
    public Transform Enemy;
    public float walkSpeed;
    public float jumpSpeed;
    public float EnemyHealth;
    private Animator animator;
    private bool DebugOffmeshLink;

    void Awake()
    {
        animator = GetComponent<Animator>();
        DebugOffmeshLink = EnemyNav.isOnOffMeshLink;
        SetZombieSpeed();
    }

    void Update()
    {
        float Distance = Vector3.Distance(Player.transform.position, Enemy.position);

        if (Distance < 1.25f)
        {
            Debug.Log("Enemigo ataca a Melee");
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
            StartCoroutine(FlashDamage());
        }
        else if(!animator.GetBool("IsDeadBool") && EnemyNav != null)
        {
            EnemyNav.SetDestination(Player.transform.position);
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }

        if (EnemyHealth <= 0 && !animator.GetBool("IsDeadBool")) 
        {
            EnemyNav.isStopped = true;
            animator.SetTrigger("IsDead");
            animator.SetBool("IsDeadBool", true);
        }

        if (EnemyNav.isOnOffMeshLink != DebugOffmeshLink)
        {
            DebugOffmeshLink = EnemyNav.isOnOffMeshLink;
            SetZombieSpeed();
        }
    }

    private void SetZombieSpeed()
    {
        if (DebugOffmeshLink) 
        {
            animator.SetTrigger("IsFalling");
            EnemyNav.speed = jumpSpeed;
        }
        else 
        { 
            EnemyNav.speed = walkSpeed;
            animator.SetTrigger("IsOnFloor");
        }
    }
    public IEnumerator FlashDamage()
    {
        GameObject cameraOffset = Player.GetNamedChild("Camera Offset");

        GameObject leftHand = cameraOffset.GetNamedChild("Left Controller").GetNamedChild("LeftHand").GetNamedChild("Hand");
        Material handsMat = leftHand.GetComponent<SkinnedMeshRenderer>().material;

        GameObject rightHand = cameraOffset.GetNamedChild("Right Controller").GetNamedChild("RightHand").GetNamedChild("Hand");

        if (leftHand && rightHand)
        {
            leftHand.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;

            rightHand.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;

            yield return new WaitForSeconds(0.5f);

            leftHand.GetComponent<SkinnedMeshRenderer>().material = handsMat;
            rightHand.GetComponent<SkinnedMeshRenderer>().material = handsMat;
        }
    }

}