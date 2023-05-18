using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractObject
{
    GameObject StartPos;
    [SerializeField] GameObject EndPos;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartPos = collision.gameObject;
            StartCoroutine("TeleportRoutine");
        }
    }
    IEnumerator TeleportRoutine()
    {
        yield return new WaitForSeconds(2f);
        StartPos.transform.position = EndPos.transform.position;
    }
}
