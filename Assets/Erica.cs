using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erica : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject infoUI;
    bool isEnter = false;
    string npcName  = "Erica";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isEnter = true;
        if (collision.gameObject.CompareTag("Player"))
            infoUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isEnter = false;
        if (collision.gameObject.CompareTag("Player"))
            infoUI.SetActive(false);
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && isEnter && !DialogueManager.instance.runner.IsDialogueRunning)
        {
            DialogueManager.instance.StartDialogue(npcName);
        }
            
    }
}
