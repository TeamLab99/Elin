using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoint;
    float timer;
    private float cnt = 0;
    public float maxCnt;
   
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();    
    }
    
    void Update() 
    {
        if (cnt==0)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        cnt++;
        for(int i=0; i<8; i++)
        {
            GameObject enemy = DataBase_Manager.instance.pool.Get(Random.Range(0,4)); // 하이리
            enemy.transform.position = spawnPoint[Random.Range(1, 3)].position;
            enemy.GetComponent<Hiry>().Init(spawnData[0]);
        }
        for (int i = 0; i < 8; i++)
        {
            GameObject enemy = DataBase_Manager.instance.pool.Get(Random.Range(4, 7)); // 식물
            enemy.transform.position = spawnPoint[Random.Range(3, spawnPoint.Length)].position;
            enemy.GetComponent<Plant>().Init(spawnData[1]);
        }
        for (int i = 0; i < 8; i++)
        {
            GameObject enemy = DataBase_Manager.instance.pool.Get(7); // 동물
            enemy.transform.position = spawnPoint[Random.Range(3, spawnPoint.Length)].position;
            enemy.GetComponent<Animal>().Init(spawnData[Random.Range(2, 4)]);
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public float speed;
    public int enemyID;
    public bool isInfection;
    public bool isAggressive;
}