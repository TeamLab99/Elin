using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Monster : Entity
{
    [SerializeField] float attackSpeed;
    [SerializeField] int skillCount;
    int[] skillIndex;

    void Start()
    {
        base.HealthUpdate();
    }

    public void UseSkill(int index)
    {
        MonsterSkill.Inst.UseSkill(index);
    }
    
/*    public void PatternSetting(int monsterId)
    {
        switch (monsterId)
        {
            case 1: d
        }
    }*/

    public IEnumerator MonsterHitEffectWithAttack(Entity player)
    {
        EffectManager.Inst.HitMotion(gameObject, true, 0.6f);
        yield return new WaitForSeconds(0.4f);
        base.Attack(player);
    }

    #region 전달자

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public void SetAttackSpeed(float amount)
    {
        attackSpeed = amount;
    }

    public int GetSkillCount()
    {
        return skillCount;
    }
    #endregion
}