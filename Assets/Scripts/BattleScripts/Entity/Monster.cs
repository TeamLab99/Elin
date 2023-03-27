using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class Monster : Entity
{
    #region 변수선언

    [Header("Attribute")]
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int skillCount;
    [SerializeField] protected int skillindex;
    [SerializeField] int[] skillType;
    [SerializeField] GameObject effect;

    protected Image scroll;

    protected Player player;
    protected float maxTime;
    protected int count;
    protected static float curTime;

    WaitForSeconds delay01 = new WaitForSeconds(0.01f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay15 = new WaitForSeconds(1.5f);
    #endregion

    protected override void Start()
    {
        base.Start();
        maxTime = attackSpeed;
        count = skillCount;
        curTime = maxTime;

        BPGameManager.Inst.SetMonster(this);
        EffectManager.Inst.SetSkillEfc(effect);
        scroll = GameObject.FindGameObjectWithTag("Gauge").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Battle_Player").GetComponent<Player>();

        StartCoroutine(TimerGauge());
    }

    protected void UseSkill(int index)
    {
        stopGauge = true;
        MobSkillManager.Inst.UseSkill(index);

        StartCoroutine(DelaySkill());
    }

    protected IEnumerator DelaySkill()
    {
        yield return delay15;
        curTime = maxTime;
        stopGauge = false;
    }

    /// <summary>
    /// 버프 적용 여부 확인
    /// </summary>
    public IEnumerator CheckBuff()
    {
        if (BuffManager.Inst.GetisAvoid())
        {
            SetAttack(0);
            BuffManager.Inst.AvoidOff();
        }
        else if (BuffManager.Inst.GetisDefense() && !BuffManager.Inst.GetisAvoid())
        {
            MinusAttack(player.GetBuffDefense());
            player.SetBuffDefenseZero();
            BuffManager.Inst.DefenseOff();
        }

        yield return null;
    }

    public IEnumerator AttackEffect()
    {
        player.HitEffectOn();
        yield return delay05;
        player.HitEffectOff();
    }

    protected IEnumerator AttackOfMonster()
    {
        yield return StartCoroutine(CheckBuff());
        EffectManager.Inst.MobAtkMotion(gameObject, 0.4f);
        StartCoroutine(AttackEffect());
        yield return delay05;
        EffectManager.Inst.CallHitCorutine(player.gameObject);
        Attack(player);
        SetAttackReturn();
    }

    protected virtual IEnumerator MonsterPattern()
    {
        if (count > 0)
        {
            curTime = maxTime;
            StartCoroutine(AttackOfMonster());
            count--;
        }
        else if (count == 0)
        {
            if (maxTime > 1f)
            {
                UseSkill(skillindex);
                maxTime = attackSpeed;
            }
            count = skillCount;
        }

        yield return null;
    }

    protected IEnumerator TimerGauge()
    {
        scroll.fillAmount = curTime / maxTime;

        if (!stopGauge)
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                StartCoroutine(MonsterPattern());
            }
        }

        yield return delay01;
        StartCoroutine(TimerGauge());
    }

    #region 전달자
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
    public int GetSkillCount()
    {
        return skillCount;
    }
    public void SetAttackSpeed(float amount)
    {
        attackSpeed = amount;
    }
    #endregion
}