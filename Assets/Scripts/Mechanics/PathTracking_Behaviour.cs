using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Random;
using static UnityEngine.GraphicsBuffer;
using Unity.XR.CoreUtils;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

public class PathTracking_Behaviour : MonoBehaviour
{
    //The position of the player
    public Transform playerPosition;

    //List of the positions and its data (Requirements to move to the next position)
    public List<PositionData> positionsData;

    //List of spawn positions where zombies will spawn
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

    public NavMeshSurface navigationMesh;

    private float step;

    private bool startedWave;


    void Start()
    {
        speed = 1;
        nextPosIndex = 0;
        nextPosition = positionsData[nextPosIndex];
        kills = 0;
        getSpawnData(positionsData[nextPosIndex]);
        step = 0.0f;
        startedWave = false;
    }

    void Update()
    {
        if (nextPosition.requirementArea.Equals(PositionData.requirement.TIMED))
        {
            //If player has to defend a position for certain time
            Movement();
            if (startedWave == true)
            {
                SpawnZombies();
            }
        }
        if (nextPosition.requirementArea == PositionData.requirement.KILLS)
        {
            //If player has to kill an amount of enemies to move forward
            KillCountCheck();
            if (startedWave == true)
            {
                SpawnZombies();
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
                startedWave = false;
                gameObject.GetComponent<AudioSource>().Play();
                MoveToNextPos();
            }
        }
    }

    private void KillCountCheck()
    {
        if (kills >= nextPosition.requirementTask)
        {
            startedWave = false;
            gameObject.GetComponent<AudioSource>().Play();
            MoveToNextPos();
        }
    }

    private void MoveToNextPos()
    {
        step += 0.05f * Time.deltaTime;
        step = Mathf.Clamp(step, 0, Mathf.PI);
        var movementSpeed = evaluatePos(step);
        playerPosition.position = Vector3.Lerp(playerPosition.position, nextPosition.position.position, movementSpeed);

        if (Vector3.Distance(playerPosition.position, nextPosition.position.position) < 0.001f)
        {
            gameObject.GetComponent<AudioSource>().Stop();
            step = 0;
            nextPosIndex += 1;
            if (nextPosIndex < positionsData.Count)
            {
                nextPosition = positionsData[nextPosIndex];
                if (startedWave == false)
                {
                    getSpawnData(nextPosition);
                    startedWave = true;

                }
                
                isMoving = false;
                kills = 0;
            }
            if (nextPosIndex >= positionsData.Count)
            {
                SceneTransitionManager.singleton.GoToSceneAsync(2);
            }
        }
    }

    public float evaluatePos(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }

    private void  SpawnZombies()
    {
        startedWave = false;

        if (nextPosition.requirementArea.Equals(PositionData.requirement.TIMED)) {
            for (int x = 0; x < nextPosition.enemyWaves * 3; x++)
            {
                foreach (Enemy_SpawnData enemySpawnData in spawnPositions)
                {
                    if (enemySpawnData.spawnCount < enemySpawnData.MaxSpawnedZombies)
                    {
                        enemySpawnData.spawnCount++;
                        int randomSpawnTime = Range(enemySpawnData.minSpawnTimeInterval, enemySpawnData.maxSpawnTimeInterval);
                        spawnCoroutine = StartCoroutine(ZombiesSpawner(randomSpawnTime, enemySpawnData));
                    }
                }
            }
        }
        else {
            for (int x = 0; x < nextPosition.enemyWaves; x++)
            {
                foreach (Enemy_SpawnData enemySpawnData in spawnPositions)
                {
                    if (enemySpawnData.spawnCount < enemySpawnData.MaxSpawnedZombies)
                    {
                        enemySpawnData.spawnCount++;
                        int randomSpawnTime = Range(enemySpawnData.minSpawnTimeInterval, enemySpawnData.maxSpawnTimeInterval);
                        StartCoroutine(ZombiesSpawner(randomSpawnTime, enemySpawnData));
                    }
                }
            }
        }
    }

    private void getSpawnData(PositionData pos)
    {
        spawnPositions = pos.GetComponentsInChildren<Enemy_SpawnData>();
    }

    private IEnumerator ZombiesSpawner(int waitTime, Enemy_SpawnData enemySpawnData)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject randomZombieType = zombieTypes[Range(0, zombieTypes.Count)];
        GameObject zombie = Instantiate(randomZombieType);
        zombie.transform.position = enemySpawnData.spawnPoint.position;
        zombie.transform.rotation = enemySpawnData.spawnPoint.rotation;
        zombie.GetComponent<EnemyAI>().Player = playerPosition.gameObject;
        zombie.GetComponent<EnemyAI>().jumpSpeed = 1f;
        zombie.GetComponent<EnemyAI>().walkSpeed = 0.1f;
       
        zombie.GetComponent<Enemy_Damageable>().PlayerPath = this;
        zombie.GetComponent<Enemy_Damageable>().Health = Range(1, 10);
        
        nextPosition.enemiesSpawned.Add(zombie);
    }
}
