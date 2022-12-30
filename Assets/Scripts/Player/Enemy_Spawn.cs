using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*enum MonType
{
    Sky = 0,
    Ground = 1
}*/


public class Enemy_Spawn : MonoBehaviour
{
    public int curCnt;
    public float curTime;
    public float spawnTime = 3f;
    public Transform[] spawnLoc;
    public GameObject enemy;

    private int spawnMon;
    private int spawnPoint;
    private int maxCnt = 5;
    void Start()
    {
        curCnt = 0;
    }

    
    void Update()
    {
        if (curTime >= spawnTime && curCnt < maxCnt)
        {
            //spawnMon = Random.Range(0, 2);
            spawnPoint = Random.Range(0, 5);
            SpawnEnemy(spawnMon, spawnPoint);
        }

        curTime += Time.deltaTime;
    }
    void SpawnEnemy(int mon, int loc)
    {
        curTime = 0;
        curCnt++;
        Instantiate(enemy, spawnLoc[loc]);
    }
}
