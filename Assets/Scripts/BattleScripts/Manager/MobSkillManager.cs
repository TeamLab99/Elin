using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSkillManager : MonoBehaviour
{
    public static MobSkillManager Inst { get; private set; }
    void Awake() => Inst = this;

    Monster monster;
    Player player;

    public void UseSkill(int index)
    {
        switch (index)
        {
            case 1: AttackSpeedUp(); break;
            default: break;
        }
    }
    public void SetEntites(Player player, Monster monster)
    {
        this.player = player;
        this.monster = monster;
    }

    public void AttackSpeedUp()
    {
        if (monster.GetAttackSpeed() > 1f)
        {
            EffectManager.Inst.MobSkillEfc();
            monster.SetAttackSpeed(monster.GetAttackSpeed()-0.5f);
        }
        else
            return;
    }
}
