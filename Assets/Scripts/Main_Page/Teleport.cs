using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject toObj;
    private GameObject target;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.gameObject;
            StartCoroutine("TeleportRoutine");
        }
    }
    IEnumerator TeleportRoutine()
    {
        yield return null;
        target.transform.position = toObj.transform.position;
    }
}
