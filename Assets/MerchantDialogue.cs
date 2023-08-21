using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantDialogue : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject infoUI;
    [SerializeField] Erica erica;

    bool isEnter;
    bool isDialogue;

    public int dialougeIndex;

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
        if (Input.GetKeyDown(KeyCode.X) && isEnter && !isDialogue)
        {
            ChooseDialogue(dialougeIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isEnter && isDialogue)
        {
            //DialogueManager.Instance.lineView.UserRequestedViewAdvancement();
        }
    }

    void ChooseDialogue(int index)
    {
        switch (index)
        {
            case 0:
                DialogueManager.Instance.runner.StartDialogue("Merchant");
                erica.SetIndex(2);
                break;
            case 1:
                DialogueManager.Instance.runner.StartDialogue("Merchant2");
                GetComponent<Merchant>().isQuestClear = true;
                erica.ericaEnd = true;
                break;
        }

        isDialogue = true;
        SetPlayerControl(false);
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

    void SetTrue()
    {
        Debug.Log("실행됨");
        SetPlayerControl(true);
    }

    public void SetIndex(int index)
    {
        dialougeIndex = index;
        isDialogue = false;
    }
}
