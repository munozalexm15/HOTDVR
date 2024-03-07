using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PathTracking_Behaviour : MonoBehaviour
{
    //The position of the player
    public Transform playerPosition;

    //List of the positions and its data (Requirements to move to the next position)
    public List<PositionData> positionsData;

    //In case the area is timed, this variable works for checking if the time is up
    private float timeInPos;

    private bool isMoving;

    //Next position with the data required
    private PositionData nextPosition;

    private int nextPosIndex;


    //Speed of the player; indicates at what speed will move from point to point
    private float speed;


    //Kill counter in case the next pos is completed by doing kills (it has to be modified from the Enemy_Damageable script)
    public int kills;


    void Start()
    {
        speed = 3;
        nextPosIndex = 0;
        nextPosition = positionsData[nextPosIndex];
    }

    void Update()
    {
        if (nextPosition.requirementArea.Equals(PositionData.requirement.TIMED))
        {
            //If player has to defend a position for certain time
            Movement();
        }
        if (nextPosition.requirementArea == PositionData.requirement.KILLS)
        {
            //If player has to kill an amount of enemies to move forward
            KillCountCheck();
        }
    }

    public void Movement()
    {
        if (nextPosition.requirementArea.Equals(PositionData.requirement.TIMED))
        {
            if (!isMoving)
            {
                timeInPos = nextPosition.requirementTask;
                isMoving = true;
            }

            timeInPos--;
            if (timeInPos <= 0)
            {
                MoveToNextPos();
                
            }
        }
    }

    private void KillCountCheck()
    {
        if (kills >= nextPosition.requirementTask)
        {
            MoveToNextPos();
            kills = 0;
        }
    }

    private void MoveToNextPos()
    {
        //on moving to second pos it fails (if it is killcount)
        var step = speed * Time.deltaTime;
        playerPosition.position = Vector3.MoveTowards(playerPosition.position, nextPosition.position.position, step);


        if (Vector3.Distance(playerPosition.position, nextPosition.position.position) < 0.001f)
        {
            nextPosIndex += 1;
            if (nextPosIndex < positionsData.Count)
            {
                nextPosition = positionsData[nextPosIndex];
                isMoving = false;
            }

        }
    }
}
