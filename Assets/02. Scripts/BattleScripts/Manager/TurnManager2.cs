using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TurnManager2 : Singleton<TurnManager2>
{
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] int startCardCount = 5;
    [SerializeField] Monster2 monster;

    [Header("Properties")]
    public bool isLoading; // 카드 사용 방지, 몬스터 공격 방지
    public bool myTurn;

    WaitForSeconds delay025 = new WaitForSeconds(0.25f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);

    public static Action OnAddCard;
    public static event Action EndDrawPhase;

    public IEnumerator StartGameCo(GameObject ui,float time = 0.5f)
    {
        isLoading = true;
        delay05 = new WaitForSeconds(time);
        yield return delay05;

        ui.SetActive(true);

        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke();
            yield return delay025;
        }
        CardManager2.instance.SetKey();

        BattleGameManager.instance.Notification("전투 시작!");
        yield return delay05;

        isLoading = false;
    }

    public IEnumerator ReDrawCards()
    {
        isLoading = true;

        yield return delay05;

        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke();
            yield return delay025;
        }
        CardManager2.instance.SetKey();

        isLoading = false;
        EndDrawPhase?.Invoke();
        // isLoading으로 엔티티들의 모든 추가 행동을 멈출 것인가?
    }
}
