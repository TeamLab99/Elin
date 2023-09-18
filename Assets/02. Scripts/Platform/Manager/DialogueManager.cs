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
    public Dictionary<string, int> dialogueIdx = new Dictionary<string, int>();
    private void Awake()
    {
        if (runner == null)
            runner = FindObjectOfType<DialogueRunner>();
        InitFunction();
    }

    public void StartDialogue(string title)
    {
        runner.StartDialogue(LoadDialouge(title));
        Managers.Input.PlayerMoveControl(false);
    }

    public string LoadDialouge(string npcName)
    {
        if (!dialogueIdx.ContainsKey(npcName))
            dialogueIdx.Add(npcName, 0);
        return npcName + dialogueIdx[npcName];
    }

    public void InitFunction()
    {
        runner.onDialogueComplete.AddListener(() => { Managers.Input.PlayerMoveControl(true); Debug.Log("작동중입니다."); });
        runner.AddCommandHandler<string, int>("Quest", AcceptQuest);
        runner.AddCommandHandler<string>("Act", SetActor); // 이미지 추가
        runner.AddCommandHandler<string>("Clear", ClearQuest); // 퀘스트 완료 or 다음 텍스트
        runner.AddCommandHandler<string>("Next", NextDialogue);
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

    public void ClearQuest(string questNpcName)
    {
        if (!dialogueIdx.ContainsKey(questNpcName))
            return;
        if (dialogueIdx[questNpcName]%2==0)
            dialogueIdx[questNpcName] += 2;
        else
            dialogueIdx[questNpcName] += 1;
    }

    public void NextDialogue(string questNpcName)
    {
        if (!dialogueIdx.ContainsKey(questNpcName))
            return;
        dialogueIdx[questNpcName] += 1;
    }
}