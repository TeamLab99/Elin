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

    [SerializeField] protected Animator animator;

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

    public void EntitiesStateChange(bool isBool)
    {
        BattleCardManager.EffectPlayBack.Invoke(isBool);
        BattleCardManager.instance.DontUseCard(isBool);
    }

    public void GaugeControl(bool _active)
    {
        BattleCardManager.EffectPlayBack.Invoke(_active);
    }

    public void DontUseCard(bool _active)
    {
        BattleCardManager.instance.DontUseCard(_active);
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

    public void AttackValue(BattleEntity entity, float value)
    {
        entity.TakeDamage(entity.battleBuffDebuff.CheckDamageImpactBuff(value));
    }

        public void ChangeAnim(EMonsterState monsterState)
    {
        switch (monsterState)
        {
            case EMonsterState.Idle:
            if(animator.GetBool("isHit") == true)
                animator.SetBool("isHit", false);
            if (animator.GetBool("isSkill") == true)  
                animator.SetBool("isSkill", false);
            if(animator.GetBool("isAttack") == true)
                animator.SetBool("isAttack", false);
                break;
            case EMonsterState.Attack:
                animator.SetBool("isAttack", true);
                break;
            case EMonsterState.Hit:
                animator.SetBool("isHit", true);
                break;
            case EMonsterState.Skill:
                animator.SetBool("isSkill", true);
                break;
            case EMonsterState.Death:
                animator.SetBool("Death", true);
                break;
        }
    }
}
