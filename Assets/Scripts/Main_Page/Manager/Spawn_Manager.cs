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
        if (cnt < maxCnt)
        {
            timer += Time.deltaTime;
            if (timer > spawnData[Random.Range(0, 2)].spawnTime)
            {
                timer = 0;
                Spawn();
            }
        }
        else
        {
            timer = 0;
        }
    }

    void Spawn()
    {
        cnt++;
        GameObject enemy = DataBase_Manager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy_Move>().Init(spawnData[Random.Range(0,2)]);
    }

    public void CountDown()
    {
        cnt -= 1;
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}