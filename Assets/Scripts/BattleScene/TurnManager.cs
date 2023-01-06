using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("Developer")]
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다")] ETurnMode eTurnMode;
    [SerializeField] [Tooltip("카드 배분이 매우 빨라집니다")] bool fastMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;

    [Header("Properties")]
    public bool myTurn;
    public bool isLoading; // 게임 끝나고 true 하면 카드, 엔티티 클릭 방지

    enum ETurnMode { Random, My, Other}
    WaitForSeconds delay05 = new WaitForSeconds(0.1f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted;

    void GameSetup()
    {
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);

        switch (eTurnMode)
        {
            case ETurnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;
        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(true);
            yield return new WaitForSeconds(0.2f);
            
        }
        yield return new WaitForSeconds(1f);

        BPGameManager.Inst.isCardMoving = false;

        StartCoroutine(StartTurnCo());
    }

    public IEnumerator StartDraw()
    {
        BPGameManager.Inst.isCardMoving = true;
        isLoading = true;
        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(true);
            yield return new WaitForSeconds(0.2f);

        }
        yield return new WaitForSeconds(1f);

        BPGameManager.Inst.isCardMoving = false;
        isLoading = false;
    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;

        if (myTurn)
            BPGameManager.Inst.Notification("나의 턴",myTurn);
        else
            BPGameManager.Inst.Notification("상대 턴",myTurn);

        yield return delay07;
        //OnAddCard?.Invoke(myTurn);
        yield return delay07;
        isLoading = false;
        OnTurnStarted?.Invoke(myTurn);

        //BPGameManager.Inst.isDelay = false;
    }

    public void EndTurn()
    {
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
    }
}
