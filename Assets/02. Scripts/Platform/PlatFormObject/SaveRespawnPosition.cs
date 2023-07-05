using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRespawnPosition : MonoBehaviour
{
    BoxCollider2D boxCollide;
    private void Awake()
    {
        boxCollide = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerRespawnManager.instance.ChangeRespawnPosition(gameObject.transform);
            boxCollide.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
