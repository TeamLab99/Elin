using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn;

public class DialogueManager : Singleton<DialogueManager>
{
    public DialogueRunner runner;
    public Image[] characterImages;
    public Dictionary<string, Sprite> spritesDic = new Dictionary<string, Sprite>();
    public Dictionary<string, int> dialogueIdx = new Dictionary<string, int>();

    private void Awake()
    {
        if (runner == null)
            runner = FindObjectOfType<DialogueRunner>();
        InitFunction();
    }

    public void StartDialogue(string title)
    {
        PlatformManager.Instance.OnOffUI();
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
        runner.onDialogueComplete.AddListener(() =>
        {
            if (!PlatformEventManager.instance.isEnding)
                Managers.Input.PlayerMoveControl(true);
            OffCharacterImage();
            PlatformManager.Instance.OnOffUI();
        }); // 대화 종료시
        runner.AddCommandHandler<string, string>("Act", SetActor); // 이미지 추가
        runner.AddCommandHandler<string>("Next", NextDialogue); // 다음 대화로 변경
        runner.AddCommandHandler("Event", SetEvent); // 이벤트 발생
        runner.AddCommandHandler<int>("Earn", EarnAum);
        runner.AddCommandHandler("EricaTf", EricaSpawn);
        runner.AddCommandHandler("ResumeBattle", ResumeBattle);
        runner.AddCommandHandler("PauseBattle", PauseBattle);
        runner.AddCommandHandler("StartNightmareBattle", StartNightmareBattle);
        runner.AddCommandHandler("BlackPanelOn", BlackPanelOn);
        runner.AddCommandHandler("FadeIn", FadeIn);
        runner.AddCommandHandler("ImmediateDeath", ImmediateDeath);
    }

    public void OffCharacterImage()
    {
        characterImages[0].gameObject.SetActive(false);
        characterImages[1].gameObject.SetActive(false);
    }

    public void SetActor(string _actorName, string _expression)
    {
        string findSource = _actorName + _expression;
        if (!spritesDic.ContainsKey(findSource))
            spritesDic.Add(findSource, Managers.Resource.Load<Sprite>("Sprites/" + findSource));
        ChooseActiveImage(spritesDic[findSource], _actorName);
    }

    public void ChooseActiveImage(Sprite image, string name)
    {
        if (!characterImages[0].gameObject.activeSelf)
        {
            characterImages[0].gameObject.SetActive(true);
            characterImages[0].sprite = image;
            characterImages[0].gameObject.name = name;
            return;
        }

        if (characterImages[0].gameObject.name == name)
        {
            characterImages[0].sprite = image;
            return;
        }

        if (!characterImages[1].gameObject.activeSelf)
        {
            characterImages[1].gameObject.SetActive(true);
            characterImages[1].sprite = image;
            characterImages[1].gameObject.name = name;
        }
    }

    public void NextDialogue(string questNpcName)
    {
        if (!dialogueIdx.ContainsKey(questNpcName))
            return;
        dialogueIdx[questNpcName] += 1;
    }

    public void SetEvent()
    {
        PlatformEventManager.instance.SetEvent();
    }

    public void ClearQuest(string questNpcName)
    {
        if (!dialogueIdx.ContainsKey(questNpcName))
            return;
        if (dialogueIdx[questNpcName] % 2 == 0)
            dialogueIdx[questNpcName] += 2;
        else
            dialogueIdx[questNpcName] += 1;
    }


    public void OffDialogueImage()
    {
        characterImages[0].gameObject.SetActive(false);
        characterImages[1].gameObject.SetActive(false);
    }

    public void EarnAum(int aum = 1000)
    {
        ItemManager.instance.EarnAum(aum);
    }

    public void EricaSpawn()
    {
        PlatformEventManager.instance.EricaSpawn();
    }

    public void StartNightmareBattle()
    {
        BattleManager.instance.StartNightmareBattle();
    }

    public void PauseBattle()
    {
        CardManager.EffectPlayBack.Invoke(true);
        CardManager.instance.DontUseCard(true);
        MagicManager.instance.monster.TimerControl(true);
    }

    public void ResumeBattle()
    {
        CardManager.EffectPlayBack.Invoke(false);
        CardManager.instance.DontUseCard(false);
        MagicManager.instance.monster.TimerControl(false);
    }

    public void BattleDialogue()
    {
        PlatformManager.Instance.OnOffUI();
        runner.StartDialogue("Erica8");
        Managers.Input.PlayerMoveControl(false);
    }

    public void ImmediateDeath()
    {
        MagicManager.instance.player.ImmediateDeath();
    }

    public void BlackPanelOn()
    {
        OnOffUI.instance.BlackPanelOn();
        BattleManager.instance.GameEnding();
    }

    public void FadeIn()
    {
        OnOffUI.instance.FadeIn();
    }
}