using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// 이것도 첫 턴 드로우 기능을 제외하고는 현재 안쓰고있는 스크립트
/// 기존 하스스톤 예제에서 상대턴 내턴 구분하려고 만든건데 나중에 손을 좀 봐서 우리 게임 입맛대로 고칠 예정
/// </summary>
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

    // 첫 턴 카드 드로우
    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;
        CardManager.Inst.SetIsCardMoving(true);

        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(true);
            yield return new WaitForSeconds(0.2f);
            
        }
        yield return delay07;

        CardManager.Inst.SetIsCardMoving(false);

        StartCoroutine(StartTurnCo());
    }

    // 카드 다 사용했을 시, 재 드로우
    public IEnumerator CardDraw()
    {
        CardManager.Inst.SetIsCardMoving(true);
        isLoading = true;
        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(true);
            yield return new WaitForSeconds(0.2f);

        }
        yield return delay07;

        CardManager.Inst.SetIsCardMoving(false);

        isLoading = false;
    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;

/*        if (myTurn)
            BPGameManager.Inst.Notification("나의 턴",myTurn);
        else
            BPGameManager.Inst.Notification("상대 턴",myTurn);*/

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
