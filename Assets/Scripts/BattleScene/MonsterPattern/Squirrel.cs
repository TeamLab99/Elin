using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : Monster
{
    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        // 게이지의 상태 계속 업데이트
        scroll.fillAmount = curTime / maxTime;

        // 누군가 죽지 않았다면 계속 검사
        if (!stopGauge)
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                if (count > 0)
                {
                    // 게이지 재충전, 어택 애니메이션, 공격 함수
                    curTime = maxTime;
                    StartCoroutine(MonsterHitEffectWithAttack());
                    count--;
                }
                else if (count == 0)
                {
                    if (maxTime > 1f)
                    {
                        stopGauge = true;
                        UseSkill(1);
                        StartCoroutine(SkillDelay());
                        maxTime = attackSpeed;
                    }
                    count = skillCount;
                }
            }
        }
    }
}
