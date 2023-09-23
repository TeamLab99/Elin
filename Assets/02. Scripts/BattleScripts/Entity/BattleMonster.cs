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
    [SerializeField] protected Sprite gaugeIcon;

    protected float maxTime;
    protected float curTime;
    protected int count;
    bool stopGauge;

    Image gauge;
    protected BattleGaugeIconAnimation iconAnimation;

    protected WaitForSeconds delay = new WaitForSeconds(0.5f);

    private void Start()
    {
        BattleCardManager.EffectPlayBack += TimerControl;

        Init();
    }

    private void OnDestroy()
    {
        BattleCardManager.EffectPlayBack -= TimerControl;
    }

    public override void Init()
    {
        delay = new WaitForSeconds(Time.deltaTime);
        maxTime = attackSpeed;
        curTime = maxTime;
        count = skillCount;
        hp = maxHp;
        battleBuffDebuff = gameObject.AddComponent<BattleBuffManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BattlePlayer>();
        StartCoroutine(GetGaugeUI("HpBar"));
    }

    public override IEnumerator GetGaugeUI(string tagName)
    {
        yield return new WaitForEndOfFrame();
        var monsterUI = GameObject.FindGameObjectWithTag("MonsterUI").GetComponent<MonsterUIContorller>();
        hpBar = monsterUI.hpBar;
        hpTMP = monsterUI.hpTMP;
        gauge = monsterUI.gauge;
        iconAnimation = monsterUI.gaugeIcon;
        
        iconAnimation.SetIcon(gaugeIcon);
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
                iconAnimation.Animation(gauge.fillAmount);
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
