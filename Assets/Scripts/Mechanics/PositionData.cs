using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData : MonoBehaviour
{
    public enum requirement { TIMED, KILLS}

    public Transform position;
    public requirement requirementArea;
    public float requirementTask;
    public int enemyWaves;

    public List<GameObject> enemiesSpawned = new List<GameObject>();
}
