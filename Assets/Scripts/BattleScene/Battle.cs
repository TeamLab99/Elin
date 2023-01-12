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
    [SerializeField] Entity monster;

    // 행동 게이지 수치 관련
    [Header("UI")]
    public Image scroll;
    float maxTime = 3f;
    static float curTime;

    // 배틀 종료 판단
    public bool isDie;

    private void Start()
    {
        // 게이지 최대시간 입력
        curTime = maxTime;
    }
    private void Update()
    {
        // 게이지의 상태 계속 업데이트
        scroll.fillAmount = curTime / maxTime;

        // 누군가 죽지 않았다면 계속 검사
        if (!isDie)
        {
            // Time.DeltaTime을 이용하여 curTime이 0초가 될 때 마다 몬스터의 공격 함수 호출
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                // 게이지 재충전, 어택 애니메이션, 공격 함수
                curTime = maxTime;
                EffectManager.Inst.MoveTransform(monster.gameObject, 
                    new PRS(monster.transform.position + Vector3.left * 25, Utils.QI, Vector3.one * 1.2f), true, 0.6f);
                Attack(10, true);
            }
        }

    }

    // 공격 함수(데미지량, 데미지를 받을 대상)
    public void Attack(int damageNum, bool isPlayer) 
    {
        // true면 플레이어, false 면 적의 Entity 정보를 가져옴
        Entity entity = isPlayer ? player : monster;
        entity.health -= damageNum;

        if (entity.health <= 0) 
        {
            entity.health = 0;
            entity.SetHealth();
            GameOver();
            return;
        }

        // 체력 text 업데이트
        entity.SetHealth(); 
    }

    // 회복 함수
    public void Heal(int num, bool isMine) 
    {
        Entity entity = isMine ? player : monster;
        entity.health += num;
        entity.SetHealth();
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

    // 몬스터 기본 공격 코루틴(현재 미사용)
    IEnumerator MonsterNormalAttack()
    {
        // 플레이어의 체력 0 이면, 코루틴 정지
        if (player.health <= 0)
        {
            StopCoroutine(MonsterNormalAttack());
            yield return null;
        }

        // 3초 딜레이 이후 Attack 함수 실행
        yield return new WaitForSeconds(3f);
        Attack(2, true);

        // 재귀 실행
        StartCoroutine(MonsterNormalAttack());
    }


}
