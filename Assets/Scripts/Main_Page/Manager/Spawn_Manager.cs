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
        if (Input.GetKeyDown(KeyCode.X))
        {
            Spawn();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnPlant();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnHiry();
        }
    }

    void Spawn()
    {
        cnt++;
        for(int i=0; i<1; i++)
        {
            GameObject enemy = DataBase_Manager.instance.pool.Get(4); // 하이리
            enemy.transform.position = spawnPoint[Random.Range(1, 3)].position;
            enemy.GetComponent<Hiry>().Init(spawnData[3]);
        }
      
       
    }
    void SpawnPlant()
    {
        for (int i = 0; i < 1; i++)
        {
            GameObject enemy = DataBase_Manager.instance.pool.Get(1); // 식물
            enemy.transform.position = spawnPoint[Random.Range(3, spawnPoint.Length)].position;
            enemy.GetComponent<Plant>().Init(spawnData[1]);
        }
    }

    void SpawnHiry()
    {
        for (int i = 0; i < 1; i++)
        {
            GameObject enemy = DataBase_Manager.instance.pool.Get(3); // 동물
            enemy.transform.position = spawnPoint[Random.Range(3, spawnPoint.Length)].position;
            enemy.GetComponent<Animal>().Init(spawnData[0]);
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