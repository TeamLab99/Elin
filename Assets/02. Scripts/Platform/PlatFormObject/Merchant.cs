using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : InteractObject
{
    [SerializeField] GameObject talkUI;
    [SerializeField] GameObject merchantUI; 
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            talkUI.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.X))
            {
                merchantUI.SetActive(true);
            }
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
