using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject EndPos;
    private GameObject StartPos;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartPos = collision.gameObject;
            StartPos.transform.position = EndPos.transform.position;
        }
    }
}
