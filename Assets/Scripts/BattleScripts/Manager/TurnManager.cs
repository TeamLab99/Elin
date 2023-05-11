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

    private void Start()
    {
        StartCoroutine(Init());
    }

    [Header("Developer")]
    [SerializeField] [Tooltip("시작 턴을 정합니다")] ETurnMode eTurnMode;
    [SerializeField] [Tooltip("카드 배분이 매우 빨라집니다")] bool fastMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다")] int startCardCount;
    [SerializeField] NotificationPanel notificationPanel;

    [Header("Properties")]
    public bool myTurn;
    public bool isLoading; // 게임 끝나고 true 하면 카드, 엔티티 클릭 방지

    enum ETurnMode { Random, My, Other }
    WaitForSeconds delay05 = new WaitForSeconds(0.1f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);
    WaitForSeconds delay02 = new WaitForSeconds(0.2f);

    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted;

    IEnumerator Init()
    {
        yield return new WaitForEndOfFrame();
        notificationPanel = GameObject.Find("NotificationPanel").GetComponent<NotificationPanel>();
        StartCoroutine(StartGameCo());
    }

    void GameSetup()
    {
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);

        switch (eTurnMode)
        {
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

        Notification("게임 시작", myTurn);

        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(true);
            yield return delay02;
        }
        yield return delay07;

        isLoading = false;
        BPGameManager.Inst.MonsterPause(false);

    }

    // 카드 다 사용했을 시, 재 드로우
    public IEnumerator CardDraw()
    {
        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(true);
            yield return delay02;
        }
        yield return delay07;

        isLoading = false;
        OnTurnStarted?.Invoke(myTurn); // EndTurnBtn 활성화/비활성화 델리게이트
    }

    // 턴 알림 팝업 텍스트 색 변경.
    public void Notification(string message, bool state)
    {
        if (state)
            notificationPanel.Show(message, new Color32(0, 255, 0, 100));
        else
            notificationPanel.Show(message, new Color32(255, 0, 0, 100));
    }
}
