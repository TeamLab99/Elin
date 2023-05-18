using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectPooling : MonoBehaviour
{
    [Tooltip("The prefab that will be used for object creation.")]
    public GameObject prefabObject;

    [Tooltip("The maximum number of objects that can be created.")]
    public int maxObjects = 10;

    [Tooltip("The size of the object pool.")]
    public int poolSize = 5;

    [Tooltip("Whether or not objects should be reset when retrieved from the pool.")]
    public bool resetObjectsOnReuse = true;

    private GameObject[] objectPool;

    private void Awake()
    {
        objectPool = new GameObject[poolSize];

        // Populate the pool with objects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObject = Instantiate(prefabObject, Vector3.zero, Quaternion.identity) as GameObject;
            objectPool[i] = newObject;
            newObject.SetActive(false);
        }
    }

    public GameObject GetObject()
    {
        // Find an object in the pool that is not currently active
        for (int i = 0; i < poolSize; i++)
        {
            if (!objectPool[i].gameObject.activeInHierarchy)
            {
                objectPool[i].gameObject.SetActive(true);
                return objectPool[i].gameObject;
            }
        }

        // If all objects in the pool are in use and the max objects has not been reached,
        // create a new object and add it to the pool
        if (objectPool.Length < maxObjects)
        {
            GameObject newObject = Instantiate(prefabObject, Vector3.zero, Quaternion.identity) as GameObject;
            GameObject poolObj = newObject;
            poolObj.gameObject.SetActive(true);
            objectPool = new GameObject[objectPool.Length + 1];
            objectPool[objectPool.Length - 1] = poolObj;
            return newObject;
        }

        // If all objects are in use and the max objects has been reached, return null
        return null;
    }

    public void ReturnObject(GameObject poolObj)
    {
        poolObj.gameObject.SetActive(false);
    }
}