using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TurnManager2 : Singleton<TurnManager2>
{
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] int startCardCount = 5;

    [Header("Properties")]
    public bool isLoading; // 카드 사용 방지, 몬스터 공격 방지
    public bool myTurn;

    WaitForSeconds delay025 = new WaitForSeconds(0.25f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    public static Action OnAddCard;

    public IEnumerator StartGameCo(GameObject ui,float time = 0.5f)
    {
        delay05 = new WaitForSeconds(time);
        yield return delay05;
        ui.SetActive(true);
        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke();
            yield return delay025;
        }
    }


    /// <summary>
    /// 상대 행동 중에 자신의 행동 금지
    /// </summary>
    /// <returns></returns>
    IEnumerator StartTurnCo()
    {
        isLoading = true;

        yield return delay05;
        // yield return 코루틴 실행, 콜백 함수 넣어야 할 듯

        isLoading = false;
    }
}
