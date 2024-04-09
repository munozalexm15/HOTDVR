using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Random;
using static UnityEngine.GraphicsBuffer;

public class PathTracking_Behaviour : MonoBehaviour
{
    //The position of the player
    public Transform playerPosition;

    //List of the positions and its data (Requirements to move to the next position)
    public List<PositionData> positionsData;

    private Enemy_SpawnData[] spawnPositions;

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

    public List<GameObject> zombieTypes;

    private Coroutine spawnCoroutine;

    private float step;


    void Start()
    {
        speed = 1;
        nextPosIndex = 0;
        nextPosition = positionsData[nextPosIndex];
        kills = 0;
        getSpawnData(positionsData[nextPosIndex]);
        step = 0.0f;
    }

    void Update()
    {
        if (nextPosition.requirementArea.Equals(PositionData.requirement.TIMED))
        {
            //If player has to defend a position for certain time
            Movement();
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine("SpawnZombies");
            }
           
        }
        if (nextPosition.requirementArea == PositionData.requirement.KILLS)
        {
            //If player has to kill an amount of enemies to move forward
            KillCountCheck();
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine("SpawnZombies");
            }
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
        }
    }

    private void MoveToNextPos()
    {
        //on moving to second pos it fails (if it is killcount)
       
        step += 0.05f * Time.deltaTime;
        var movementSpeed = Mathf.Lerp(0, speed, step);
        playerPosition.position = Vector3.MoveTowards(playerPosition.position, nextPosition.position.position, movementSpeed);

        if (Vector3.Distance(playerPosition.position, nextPosition.position.position) < 0.001f)
        {
            nextPosIndex += 1;
            if (nextPosIndex < positionsData.Count)
            {
                nextPosition = positionsData[nextPosIndex];
                getSpawnData(nextPosition);
               
                isMoving = false;
                kills = 0;
            }

        }
    }

    private IEnumerator SpawnZombies()
    {
        foreach (Enemy_SpawnData enemySpawnData in spawnPositions)
        {
            int randomSpawnTime = Range(enemySpawnData.minSpawnTimeInterval, enemySpawnData.maxSpawnTimeInterval);

            yield return new WaitForSeconds(randomSpawnTime);

            GameObject randomZombieType = zombieTypes[Range(0, zombieTypes.Count)];
            GameObject zombie = Instantiate(randomZombieType, enemySpawnData.spawnPoint);
            zombie.GetComponent<EnemyAI>().Player = playerPosition;
            zombie.GetComponent<Enemy_Damageable>().PlayerPath = this;
            enemySpawnData.spawnCount++;
        }
    }

    private void getSpawnData(PositionData pos)
    {
        spawnPositions = pos.GetComponentsInChildren<Enemy_SpawnData>();
    }
}
