using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn;

public class DialogueManager : Singleton<DialogueManager>
{
    public DialogueRunner runner;
    private void Awake()
    {
        if (runner == null)
            runner = FindObjectOfType<DialogueRunner>();
        InitFunction();
    }

    public void StartDialogue(string title)
    {
        runner.StartDialogue(title);
        Managers.Input.PlayerMoveControl(false);
    }

    public void InitFunction()
    {
        runner.onDialogueComplete.AddListener(() => { Managers.Input.PlayerMoveControl(true); Debug.Log("작동중입니다."); });
        runner.AddCommandHandler("Test", Test);
        runner.AddCommandHandler<string, int>("Quest", AcceptQuest);
    }

    public void Test()
    {
        Debug.Log("테스트중입니다.");
    }

    public void AcceptQuest<T>(string questType, T detail)
    {
        QuestManager.instance.Quest(questType, detail);
    }
}