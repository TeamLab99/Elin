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
    [SerializeField] int[] skillType;

    [Header("UI")]
    public Image scroll;

    protected Player player;
    protected float maxTime;
    protected int count;
    protected static float curTime;

    WaitForSeconds delay04 = new WaitForSeconds(0.4f);
    WaitForSeconds delay15 = new WaitForSeconds(1.5f);
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        HPTxtUpdate();
        maxTime = attackSpeed;
        count = skillCount;
        curTime = maxTime;
    }



    protected void UseSkill(int index)
    {
        MonsterSkill.Inst.UseSkill(index);
    }

    protected IEnumerator SkillDelay()
    {
        yield return delay15;
        curTime = maxTime;
        stopGauge = false;
    }

    protected IEnumerator MonsterHitEffectWithAttack()
    {
        EffectManager.Inst.HitMotion(gameObject, true, 0.6f);
        yield return delay04;
        Attack(player);
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
    public void SetPlayer(Player player)
    {
        this.player = player;
    }
    #endregion
}