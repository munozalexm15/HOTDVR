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
    private Coroutine handsStatus;
    private float defaultWalkSpeed;

    void Awake()
    {
        animator = GetComponent<Animator>();
        DebugOffmeshLink = EnemyNav.isOnOffMeshLink;
        SetZombieSpeed();
    }

    void Update()
    {
        float Distance = Vector3.Distance(Player.transform.position, Enemy.position);
       
        if (EnemyNav != null)
        {
            print(EnemyNav.destination + " " + Player.transform.position);
            EnemyNav.SetDestination(Player.transform.position); 
        }

        if (Distance < 1.25f && handsStatus == null && !animator.GetBool("IsDeadBool"))
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
            handsStatus = StartCoroutine(FlashDamage());
        }

        else if (!animator.GetBool("IsDeadBool") && Distance > 1.25f)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }

        if (EnemyHealth <= 0 && !animator.GetBool("IsDeadBool"))
        {
            EnemyNav.isStopped = true;
            animator.SetTrigger("IsDead");
            animator.SetBool("IsDeadBool", true);
            Player.GetComponent<PathTracking_Behaviour>().kills += 1;
        }

        if (EnemyNav.isOnOffMeshLink != DebugOffmeshLink)
        {
            DebugOffmeshLink = EnemyNav.isOnOffMeshLink;
            SetZombieSpeed();
        }
    }

    private void SetZombieSpeed()
    {
        print(EnemyNav.speed);
        defaultWalkSpeed = EnemyNav.speed;
        if (DebugOffmeshLink)
        {
            animator.SetTrigger("IsFalling");
            EnemyNav.speed = 1;
        }
        else
        {
            EnemyNav.speed = defaultWalkSpeed;
            animator.SetTrigger("IsOnFloor");
        }
    }
    public IEnumerator FlashDamage()
    {
        GameObject cameraOffset = Player.GetNamedChild("Camera Offset");
        GameObject leftHand = cameraOffset.GetNamedChild("Left Controller").GetNamedChild("LeftHand").GetNamedChild("Hand");
        Color handsMat = leftHand.GetComponent<SkinnedMeshRenderer>().material.color;
        GameObject rightHand = cameraOffset.GetNamedChild("Right Controller").GetNamedChild("RightHand").GetNamedChild("Hand");
        
        Player.GetComponent<PlayerHealth_Behavior>().health -= 1;
        Player.GetComponent<PlayerHealth_Behavior>().hud_health.text =  "HEALTH: " + Player.GetComponent<PlayerHealth_Behavior>().health;

        if (leftHand)
        {
            leftHand.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
        }

        if (rightHand)
        {
            rightHand.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
        }

        yield return new WaitForSeconds(1f);

        if (leftHand)
        {
            leftHand.GetComponent<SkinnedMeshRenderer>().material.color = handsMat;
        }

        if (rightHand)
        {
            rightHand.GetComponent<SkinnedMeshRenderer>().material.color = handsMat;
        }

        handsStatus = null;
      
       
    }

}