using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster2 : Entity2
{
    [SerializeField] public int attackSpeed;
    [SerializeField] public int skillCount;
    float maxTime;
    float curTime;
    bool stopGauge;

    Image gauge;
    WaitForSeconds delay001 = new WaitForSeconds(0.01f);

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
    }

    protected IEnumerator TimerGauge()
    {
        gauge.fillAmount = curTime / maxTime;

        if (!stopGauge)
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                Debug.Log("공격!");
                curTime = maxTime;
            }
        }

        yield return null;
        StartCoroutine(TimerGauge());
    }

    public void StartBattle()
    {
        StartCoroutine(TimerGauge());
    }
}
