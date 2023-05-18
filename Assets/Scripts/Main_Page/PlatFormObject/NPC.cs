using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
public class NPC : InteractObject
{
    [SerializeField] string npcName;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("대화 가능");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (Input.GetKey(KeyCode.Q)))
        {
            if(!dialogueRunner.IsDialogueRunning)
                dialogueRunner.StartDialogue(npcName);
        }
    }

}
