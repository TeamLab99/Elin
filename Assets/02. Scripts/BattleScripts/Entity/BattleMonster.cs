using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMonster : BattleEntity
{
    [SerializeField] int attackSpeed;
    [SerializeField] int skillCount;
    [SerializeField] EMonsterState monsterState;

    float maxTime;
    float curTime;
    bool stopGauge;

    Image gauge;
    WaitForSeconds delay = new WaitForSeconds(0.0001f);

    enum EMonsterState { Stop, Idle, Attack, Skill };

    public override void Init()
    {
        maxTime = attackSpeed;
        curTime = maxTime;
        battleBuffDebuff = gameObject.AddComponent<BattleBuffManager>();
        StartCoroutine(GetGaugeUI());
    }

    public IEnumerator GetGaugeUI()
    {
        yield return new WaitForEndOfFrame();
        gauge = GameObject.FindGameObjectWithTag("Gauge").GetComponent<Image>();
        StartCoroutine(GaugeTimer());
    }

    protected IEnumerator GaugeTimer()
    {
        gauge.fillAmount = curTime / maxTime;

        if (!stopGauge)
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                // 몬스터 패턴
                Debug.Log("공격!"); 
                curTime = maxTime;
            }
        }
        yield return delay;
        StartCoroutine(GaugeTimer());
    }

    public void StartBattle()
    {
        StartCoroutine(GaugeTimer());
    }

    public override void BuffCheck()
    {
        throw new System.NotImplementedException();
    }
}
