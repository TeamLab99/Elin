using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// 행동 순서 제어
/// </summary>
public class BattleTurnManager : Singleton<BattleTurnManager>
{
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] int startCardCount = 5;

    [Header("Properties")]
    public bool isLoading; // 카드 사용 방지, 몬스터 공격 방지
    public bool myTurn;

    WaitForSeconds delay025 = new WaitForSeconds(0.25f);
    WaitForSeconds delay035 = new WaitForSeconds(0.35f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);

    public static Action OnAddCard;
    //public static event Action EndDrawPhase;

    public IEnumerator StartGameCo(GameObject ui,BattleMonster monster,BattlePlayer player,float time = 0.5f)
    {
        player.SetStat(PlayerStatManager.instance.playerStatData);
        isLoading = true;
        delay05 = new WaitForSeconds(time);
        yield return delay05;
        player.GetComponent<PlayerController>().enabled = false;

        ui.SetActive(true);
        BattleGameManager.PlatformUIControlForBattle?.Invoke();

        StartCoroutine(BattleCardManager.instance.GetCost());

        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke();
            yield return delay025;
        }

        yield return delay035;


        BattleGameManager.instance.Notification("전투 시작!");
        BattleCardManager.instance.SetKey();
        yield return delay05;
        
        monster.enabled = true;
        player.enabled = true;

        isLoading = false;
    }

    public IEnumerator ReDrawCards()
    {
        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke();
            yield return delay025;
        }

        yield return delay035;
        BattleCardManager.instance.SetKey();
        isLoading = false;
    }
}