using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Monster : Entity
{
    [SerializeField] float attackSpeed;
    [SerializeField] int skillCount;

    void Start()
    {
        base.HealthUpdate();
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public int GetSkillCount()
    {
        return skillCount;
    }

    public void MonsterHitEffectWithAttack(Entity player)
    {
        EffectManager.Inst.MoveTransform(gameObject, true, 0.6f);
        base.Attack(player);
    }
}