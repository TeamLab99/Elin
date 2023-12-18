using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private bool test;
    
    private void Start()
    {
        if (test)
        {
            player.transform.position = spawnPoint.transform.position;
        }
    }
}
