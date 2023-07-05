using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class BattleMonster : BattleEntity
{
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int skillCount;
    [SerializeField] protected BattlePlayer player;
    [SerializeField] Sprite gaugeIcon;

    protected float maxTime;
    protected float curTime;
    protected int count;
    bool stopGauge;

    Image gauge;
    protected BattleGaugeIconAnimation iconAnimation;

    protected WaitForSeconds delay = new WaitForSeconds(0.5f);

    public override void Init()
    {
        delay = new WaitForSeconds(Time.deltaTime);
        maxTime = attackSpeed;
        curTime = maxTime;
        count = skillCount;
        battleBuffDebuff = gameObject.AddComponent<BattleBuffManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BattlePlayer>();
        StartCoroutine(GetGaugeUI("HpBar"));
    }

    public override IEnumerator GetGaugeUI(string tagName)
    {
        yield return new WaitForEndOfFrame();
        hpBar = GameObject.FindGameObjectsWithTag(tagName)[1].GetComponent<Image>();
        hpTMP = GameObject.FindGameObjectsWithTag("HpText")[1].GetComponent<TMP_Text>();
        gauge = GameObject.FindGameObjectWithTag("Gauge").GetComponent<Image>();
        iconAnimation = GameObject.FindGameObjectWithTag("Battle_GaugeIcon").GetComponent<BattleGaugeIconAnimation>();
        
        iconAnimation.SetIcon(gaugeIcon);
        iconAnimation.SetPlayTime(attackSpeed);
        iconAnimation.Animation(maxTime);

        HpTextUpdate();

        StartCoroutine(GaugeTimer());
    }

    protected virtual IEnumerator GaugeTimer()
    {
        while (true)
        {
            if (!stopGauge)
            {

                gauge.fillAmount = curTime / maxTime;
                curTime -= Time.deltaTime;


                if (curTime <= 0)
                {
                    StartCoroutine(MonsterPattern(skillCount));
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual IEnumerator MonsterPattern(int skillCount)
    {
        EntitiesStateChange(true);
        gameObject.transform.DOScale(Vector3.one, 0.5f).SetRelative().SetEase(Ease.Flash, 2, 0);
        Attack(player);
        EntitiesStateChange(false);
        yield return null;
    }

    protected void EntitiesStateChange(bool isBool)
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
