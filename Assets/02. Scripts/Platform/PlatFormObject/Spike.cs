using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private bool collidePlayer = false;
    private GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            collidePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidePlayer = false;
        }
    }

    private void Update()
    {
        if (collidePlayer)
        {
            if (player.transform.position.x < gameObject.transform.position.x)
                PlayerStatManager.instance.DamagePlayer(10, false);
            else
                PlayerStatManager.instance.DamagePlayer(10, true);
        }
    }
}
