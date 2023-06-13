using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster2 : Entity2
{
    [SerializeField] int attackSpeed;
    [SerializeField] int skillCount;
    [SerializeField] EMonsterState monsterState;

    float maxTime;
    float curTime;
    bool stopGauge;

    Image gauge;
    WaitForSeconds delay001 = new WaitForSeconds(0.01f);

    enum EMonsterState { Stop, Idle, Attack, Skill };

    public override void Init()
    {
        maxTime = attackSpeed;
        curTime = maxTime;
    }

    public IEnumerator GetGaugeUI()
    {
        yield return new WaitForEndOfFrame();
        gauge = GameObject.FindGameObjectWithTag("Gauge").GetComponent<Image>();
    }

    public override void BuffCheck()
    {
        throw new System.NotImplementedException();
        // 갖고 있는 버프/디버프 리스트 배틀 매니저에 반환?
        // 턴매니저 == 배틀 매니저?
    }

    protected IEnumerator GaugeTimer()
    {
        gauge.fillAmount = curTime / maxTime;

        if (monsterState == EMonsterState.Idle)
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                Debug.Log("공격!");
                curTime = maxTime;
            }
        }

        yield return null;
        StartCoroutine(GaugeTimer());
    }

    public void StartBattle()
    {
        StartCoroutine(GaugeTimer());
    }
}
