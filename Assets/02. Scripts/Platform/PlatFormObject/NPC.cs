using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractObject
{
    [SerializeField] GameObject talkUI;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            talkUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            talkUI.SetActive(false);
        }
    }
}
