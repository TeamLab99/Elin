using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private bool collidePlayer = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collidePlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collidePlayer = false;
    }

    private void Update()
    {
        if (collidePlayer)
            PlayerStatManager.instance.DamagePlayer(10);
    }
}
