using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn;

public class DialogueManager : Singleton<DialogueManager>
{
    public DialogueRunner runner;
    public Image characterImage;
    public Dictionary<string,Sprite> spritesDic = new Dictionary<string, Sprite>();
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
        runner.AddCommandHandler<string>("Act", SetActor); // 이미지 추가
    }

    public void Test()
    {
        Debug.Log("테스트중입니다.");
    }

    public void AcceptQuest<T>(string questType, T detail)
    {
        QuestManager.instance.Quest(questType, detail);
    }

    public void SetActor(string _actorName)
    {
        if (!spritesDic.ContainsKey(_actorName))
        {
           characterImage.sprite = Managers.Resource.Load<Sprite>(_actorName);
            if (Managers.Resource.Load<Sprite>(_actorName) == null)
                return;
            spritesDic.Add(_actorName, characterImage.sprite);
        }
        else
        {
            characterImage.sprite = spritesDic[_actorName];
        }
    }
}