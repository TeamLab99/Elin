using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject pullInformation;
    public Bridge bridge;
    Animator anim;
    Lever lever;
    bool collidePlayer = false;
    bool pullLever = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        lever = GetComponent<Lever>();
    }

    private void Update()
    {
        if (collidePlayer)
        {
            if (Input.GetKeyDown(KeyCode.X) && !pullLever)
            {
                anim.SetBool("Active", true);
                pullLever = true;
                ActiveLever();
                lever.enabled=false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pullInformation.SetActive(true);
            collidePlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pullInformation.SetActive(false);
            collidePlayer = false;
        }
    }

    public void ActiveLever()
    {
        bridge.LayBridge();
        PlatformEventManager.instance.SetEvent();
        DialogueManager.instance.ClearQuest("Erica");
    }
}
