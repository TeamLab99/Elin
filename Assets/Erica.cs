using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erica : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject infoUI;
    bool isEnter;
    bool isDialogue;

    int dialougeIndex = 0;

    private void Start()
    {
        DialogueManager.instance.dialogueRunner.onDialogueComplete.AddListener(SetTrue);
    }

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
                DialogueManager.instance.dialogueRunner.StartDialogue("Erica");
                break;
            case 1:
                DialogueManager.instance.dialogueRunner.StartDialogue("Erica2");
                break;
        }

        isDialogue = true;
        SetPlayerControl(false);
    }

    void SetTrue()
    {

        SetPlayerControl(true);
    }

    void SetPlayerControl(bool isBool)
    {
        if (isBool)
        {
            player.GetComponent<PlayerAbilityController>().enabled = true;
            player.GetComponent<PlayerController>().ControlPlayer(true);
            BattleGameManager.PlatformUIControlForDialouge?.Invoke();

        }
        else
        {
            player.GetComponent<PlayerController>().ControlPlayer(false);
            player.GetComponent<PlayerController>().anim.SetBool("Walk", false);
            player.GetComponent<PlayerAbilityController>().enabled = false;

            BattleGameManager.PlatformUIControlForDialouge?.Invoke();
        }
    }

    public void SetIndex(int index)
    {
        dialougeIndex = index;
        isDialogue = false;
    }
}
