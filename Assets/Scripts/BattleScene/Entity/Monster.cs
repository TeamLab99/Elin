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
    [SerializeField] GameObject effect;

    protected Image scroll;

    protected Player player;
    protected float maxTime;
    protected int count;
    protected static float curTime;

    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay15 = new WaitForSeconds(1.5f);
    #endregion

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        BPGameManager.Inst.SetMonster(this);
        EffectManager.Inst.SetSkillEfc(effect);
        HPTxtUpdate();


        var gauge = GameObject.FindGameObjectWithTag("Gauge");
        var bp = GameObject.FindGameObjectWithTag("Battle_Player");
        scroll = gauge.GetComponent<Image>();
        player = bp.GetComponent<Player>();
        maxTime = attackSpeed;
        count = skillCount;
        curTime = maxTime;
    }



    protected void UseSkill(int index)
    {
        MobSkillManager.Inst.UseSkill(index);
    }

    protected IEnumerator SkillDelay()
    {
        yield return delay15;
        curTime = maxTime;
        stopGauge = false;
    }

    protected IEnumerator MonsterHitEffectWithAttack()
    {
        EffectManager.Inst.AtkMotion(gameObject, player.gameObject.transform.parent.position ,true, 0.4f);
        player.HitEffectOn();

        yield return delay05;

        EffectManager.Inst.CallHitCorutine(player.gameObject);
        Attack(player);
        player.HitEffectOff();
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