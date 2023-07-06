using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantDialogue : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject infoUI;
    bool isQuestClear;
    bool isEnter;
    bool isDialogue;

    public int dialougeIndex;
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
        if (Input.GetKeyDown(KeyCode.X) && isEnter && !isDialogue)
        {
            ChooseDialogue(dialougeIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isEnter && isDialogue)
        {
            DialogueManager.instance.lineView.UserRequestedViewAdvancement();
        }
    }

    void ChooseDialogue(int index)
    {
        switch (index)
        {
            case 0:
                DialogueManager.instance.dialogueRunner.StartDialogue("MerChant");
                break;
            case 1:
                DialogueManager.instance.dialogueRunner.StartDialogue("MerChant2");
                break;
        }

        isDialogue = true;
        //SetPlayerControl(false);
    }
}
