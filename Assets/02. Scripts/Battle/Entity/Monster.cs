using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class Monster : Entity
{
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int skillCount;
    [SerializeField] protected Player player;
    [SerializeField] protected Sprite gaugeIcon;

    [SerializeField] protected Animator animator;

    protected float maxTime;
    protected float curTime;
    protected int count;
    protected float lastingDamageMainTainTime;
    protected float fireMainTainTime;
    protected float debuffMainTainTime;
    bool stopGauge;
    
    private Coroutine downSpeed;
    private Coroutine burn;
    private Coroutine drowning;
    private Coroutine timer;

    protected Image gauge;
    protected BattleGaugeIconAnimation iconAnimation;

    protected WaitForSeconds delay = new WaitForSeconds(0.5f);

    private void Start()
    {
        CardManager.EffectPlayBack += TimerControl;

        Init();
    }

    private void OnDestroy()
    {
        CardManager.EffectPlayBack -= TimerControl;
    }

    public override void Init()
    {
        delay = new WaitForSeconds(Time.deltaTime);
        maxTime = attackSpeed;
        curTime = maxTime;
        count = skillCount;
        hp = maxHp;
        buffDebuff = gameObject.AddComponent<BuffManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

        timer = StartCoroutine(GaugeTimer());
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
        CardManager.EffectPlayBack.Invoke(isBool);
        CardManager.instance.DontUseCard(isBool);
    }

    public void StopTimer(bool isBool)
    {
        if(isBool)
            StopCoroutine(timer);
        else
        {
            timer = StartCoroutine(GaugeTimer());
        }
    }

    public void GaugeControl(bool _active)
    {
        CardManager.EffectPlayBack.Invoke(_active);
    }

    public void DontUseCard(bool _active)
    {
        CardManager.instance.DontUseCard(_active);
    }

    public void StartBattle()
    {
        timer = StartCoroutine(GaugeTimer());
        EntitiesStateChange(false);
    }

    public override void TimerControl(bool isStop)
    {
        stopGauge = isStop;
        iconAnimation.TimerControl(isStop);
    }

    public override void Attack(Entity entity)
    {
        entity.TakeDamage(entity.buffDebuff.CheckDamageImpactBuff(attack));
    }

    public void AttackValue(Entity entity, float value)
    {
        entity.TakeDamage(entity.buffDebuff.CheckDamageImpactBuff(value));
    }

    public void ChangeAnim(EMonsterState monsterState)
    {
        switch (monsterState)
        {
            case EMonsterState.Idle:
                if (animator.GetBool("isHit") == true)
                    animator.SetBool("isHit", false);
                if (animator.GetBool("isSkill") == true)
                    animator.SetBool("isSkill", false);
                if (animator.GetBool("isAttack") == true)
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
            case EMonsterState.Run:
                animator.SetBool("Run", true);
                break;
        }
    }

    public IEnumerator DownSpeed(float time, float value)
    {
        debuffMainTainTime = time;
        maxTime += (maxTime * (value/100));
        
        //curTime= maxTime;
        
        while (debuffMainTainTime > 0)
        {
            yield return new WaitForSeconds(1f);
            debuffMainTainTime -= 1f;
        }
        
        debuffMainTainTime = 0;
        maxTime = attackSpeed;
    }

    public void DownSpeedReset(float time, float value)
    {
        if (downSpeed != null)
        {
            StopCoroutine(downSpeed);

            debuffMainTainTime = 0;
            maxTime = attackSpeed;

            downSpeed = StartCoroutine(DownSpeed(time, value));
        }
        else
        {
            downSpeed = StartCoroutine(DownSpeed(time, value));
        }
    }

    public IEnumerator DrowningDamage(float time)
    {
        lastingDamageMainTainTime = time;
        var damage = maxHp * 0.005f;
        
        if(damage < 1)
            damage = 1;
        
        while (lastingDamageMainTainTime > 0)
        {
            yield return new WaitForSeconds(1f);
            lastingDamageMainTainTime -= 1f;
            TakeDamage(damage);
        }
        
        lastingDamageMainTainTime = 0;
    }

    public void DrowningReset(float time)
    {
        if(drowning != null)
        {
            StopCoroutine(drowning);
            lastingDamageMainTainTime = 0;
            
            drowning = StartCoroutine(DrowningDamage(time));
        }
        else
        {
            drowning = StartCoroutine(DrowningDamage(time));
        }
    }
    
    public IEnumerator Burn(float time)
    {
        fireMainTainTime = time;
        var damage = attack * 0.05f;
        attack -= attack * 0.05f;
        
        if(damage < 1)
            damage = 1;
        
        while (fireMainTainTime > 0)
        {
            yield return new WaitForSeconds(1f);
            fireMainTainTime -= 1f;
            TakeDamage(damage);
        }

        fireMainTainTime = 0;
    }

    public void BurnReset(float time)
    {
        if(burn != null)
        {
            StopCoroutine(burn);
            fireMainTainTime = 0;
            
            burn = StartCoroutine(Burn(time));
        }
        else
        {
            burn = StartCoroutine(Burn(time));
        }
    }
}