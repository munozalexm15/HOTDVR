using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SpawnData : MonoBehaviour
{
    public Transform spawnPoint;

    //The minimum time to wait after a zombie has spawned
    public int minSpawnTimeInterval;

    //The maximum time to wait after a zombie has spawned
    public int maxSpawnTimeInterval;

    //Counter to know how many zombies have already spawned in this place 
    public int spawnCount;

    //Maximum quantity of zombies that can spawn in this place (it will be ignored if the area requirement is timed and not for killing zombies)
    public int MaxSpawnedZombies;


}
