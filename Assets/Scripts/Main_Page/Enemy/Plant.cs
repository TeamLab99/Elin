using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    public Transform spikeTf;
    void FixedUpdate()
    {
        if (dirX != 0)
            gameObject.transform.localScale = new Vector3(dirX, 1, 1);
    }
    protected override void Think()
    {
        dirX = Random.Range(-1, 2);
        Invoke("Think", 3f);
    }
    void SpawnObstruction()
    {
        GameObject spike = DataBase_Manager.instance.pool.GetExtra(0); // 식물
        spike.transform.position = spikeTf.position;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.gameObject;
            SpawnObstruction();
        }
    }
}
