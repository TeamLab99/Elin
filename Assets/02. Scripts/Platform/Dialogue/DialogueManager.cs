using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn;

public class DialogueManager : MonoBehaviour
{
    static DialogueManager dialogueManager;
    public DialogueRunner runner;
    public static DialogueManager Instance { get { return dialogueManager; } }
    private void Awake()
    {
        if (dialogueManager==null)
        {
            GameObject go = GameObject.Find("DialogueManager");
            if (go == null)
            {
                go = new GameObject { name = "@DialogueManager" };
                go.AddComponent<DialogueManager>();
            }
            dialogueManager = go.GetComponent<DialogueManager>();
            if (runner == null)
                runner = FindObjectOfType<DialogueRunner>();
        }
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
    }

    public void Test()
    {
        Debug.Log("테스트중입니다.");
    }
}