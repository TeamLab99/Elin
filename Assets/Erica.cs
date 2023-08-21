using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erica : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject infoUI;
    [SerializeField] MerchantDialogue merchant;
    bool isEnter;
    bool isDialogue;

    int dialougeIndex = 0;

    public bool ericaEnd;

    private void Start()
    {
        DialogueManager.Instance.runner.onDialogueComplete.AddListener(SetTrue);
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
        if (Input.GetKeyDown(KeyCode.X) && isEnter && !DialogueManager.Instance.runner.IsDialogueRunning)
        {
            if (!isDialogue)
                ChooseDialogue(dialougeIndex);
            else
                DialogueManager.Instance.StartDialogue("talkAgain");
        }

        

        if (Input.GetKeyDown(KeyCode.Space) && isEnter && isDialogue)
        {
           // DialogueManager.Instance.lineView.UserRequestedViewAdvancement();
        }
    }

    void ChooseDialogue(int index)
    {
        switch (index)
        {
            case 0:
                DialogueManager.Instance.StartDialogue("Erica");
                break;
            case 1:
                DialogueManager.Instance.StartDialogue("Erica2");
                break;
            case 2:
                DialogueManager.Instance.StartDialogue("Erica3");
                //ItemManager.instance.EarnAum();
                merchant.SetIndex(1);
                break;
        }

        isDialogue = true;
        SetPlayerControl(false);
    }

    void SetTrue()
    {
        if (!ericaEnd)
        {
            Debug.Log("실행됨");
            SetPlayerControl(true);
        }
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
