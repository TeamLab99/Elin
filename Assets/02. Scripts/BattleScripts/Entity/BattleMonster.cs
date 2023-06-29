using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BattleMonster : BattleEntity
{
    [SerializeField] int attackSpeed;
    [SerializeField] int skillCount;
    [SerializeField] EMonsterState monsterState;
    [SerializeField] BattlePlayer player;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BattlePlayer>();
        StartCoroutine(GetGaugeUI());
    }

    public IEnumerator GetGaugeUI()
    {
        yield return new WaitForEndOfFrame();
        gauge = GameObject.FindGameObjectWithTag("Gauge").GetComponent<Image>();
        StartCoroutine(GaugeTimer());
    }

    protected virtual IEnumerator GaugeTimer()
    {
        gauge.fillAmount = curTime / maxTime;

        if (!stopGauge)
        {
            curTime -= Time.deltaTime;

            if (curTime <= 0)
            {
                StartCoroutine(MonsterPattern());
                curTime = maxTime;
            }

        }
        yield return delay;
        StartCoroutine(GaugeTimer());
    }

    protected virtual IEnumerator MonsterPattern()
    {
        EntitiesStateChange(true);
        gameObject.transform.DOScale(Vector3.one,0.5f).SetRelative().SetEase(Ease.Flash, 2,0);
        Attack(player);
        EntitiesStateChange(false);
        yield return null;
    }
    
    void EntitiesStateChange(bool isBool)
    {
        BattleCardManager.EffectPlayBack.Invoke(isBool);
        BattleCardManager.instance.DontUseCard(isBool);
    }

    public void StartBattle()
    {
        StartCoroutine(GaugeTimer());
    }

    public override void TimerControl(bool isStop)
    {
        stopGauge = isStop;
    }

    public override void Attack(BattleEntity entity)
    {
        entity.TakeDamage(entity.battleBuffDebuff.CheckDamageImpactBuff(attack));
    }
}
