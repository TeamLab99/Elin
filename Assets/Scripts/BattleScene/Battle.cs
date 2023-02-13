using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 배틀 과정을 관리하는 스크립트.
/// 공격, 방어, 힐 같은 전투 관련 행동들을 작성할 예정이다.
/// </summary>

public class Battle : MonoBehaviour
{
    // 싱글턴 선언
    public static Battle Inst { get; private set; } 
    void Awake() => Inst = this;

    // 인스펙터 창에서 Entity 받아오기
    [Header("Entity")]
    [SerializeField] Entity player;
    [SerializeField] Monster monster;

    // 행동 게이지 수치 관련
    [Header("UI")]
    public Image scroll;
    float maxTime;
    static float curTime;
    int count;
    WaitForSeconds delay15 = new WaitForSeconds(1.5f);

    // 배틀 종료 판단
    public bool isDie;
    bool gaugeStop;

    private void Start()
    {
        maxTime = monster.GetAttackSpeed();
        // 게이지 최대시간 입력
        curTime = maxTime;
        count = monster.GetSkillCount();
    }
    private void Update()
    {
        // 게이지의 상태 계속 업데이트
        scroll.fillAmount = curTime / maxTime;

        // 누군가 죽지 않았다면 계속 검사
        if (!isDie)
        {
            if(!gaugeStop)
                curTime -= Time.deltaTime;
            
            if (curTime <= 0)
            {
                if (count > 0)
                {
                    // 게이지 재충전, 어택 애니메이션, 공격 함수
                    curTime = maxTime;
                    monster.MonsterHitEffectWithAttack(player);
                    count--;
                }
                else if (count == 0)
                {
                    if (maxTime > 1f)
                    {
                        gaugeStop = true;
                        isDie = true;
                        maxTime -= 0.5f;
                        EffectManager.Inst.MonsterSkillEffectOn();
                        StartCoroutine(MonsterSkillDelay());
                    }
                    count = monster.GetSkillCount();
                }
            }
        }

    }

    public IEnumerator MonsterSkillDelay()
    {
        yield return delay15;

        curTime = maxTime;

        gaugeStop = false;
        isDie = false;
    }

    public void GameOver()
    {
        // 턴 및 카드선택 정지
        TurnManager.Inst.isLoading = true;
        CardManager.Inst.SetIsCardMoving(true);
        CardManager.Inst.AllEnlargeCancle();
        isDie = true;

        // 게임 오버 알림
        Debug.Log("게임 오버!");
    }

    public void PlayerAttack()
    {
        player.Attack(monster);

        
    }

    public void PlayerHeal()
    {
        player.Heal(5);
    }
}
