using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize;
    public float returnTime;

    public Queue<GameObject> objectQueue = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            objectQueue.Enqueue(obj);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            GetPooledObject();
    }

    public GameObject GetPooledObject()
    {
        if (objectQueue.Count > 0)
        {
            GameObject obj = objectQueue.Dequeue();
            obj.SetActive(true);

            // 타이머 설정
            ReturnTimer returnTimer = obj.AddComponent<ReturnTimer>();
            returnTimer.returnTime = returnTime;

            return obj;
        }
        else
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(true);

            // 타이머 설정
            ReturnTimer returnTimer = obj.AddComponent<ReturnTimer>();
            returnTimer.returnTime = returnTime;

            return obj;
        }
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectQueue.Enqueue(obj);
    }
}


public class ReturnTimer : MonoBehaviour
{

    public float returnTime;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > returnTime)
        {
            pool objectPool = FindObjectOfType<pool>();
            objectPool.ReturnObjectToPool(gameObject);
            Destroy(this);
        }
    }
}