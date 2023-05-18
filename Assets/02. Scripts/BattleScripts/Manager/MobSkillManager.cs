using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSkillManager : MonoBehaviour
{
    public static MobSkillManager Inst { get; private set; }
    void Awake() => Inst = this;

    Monster monster;
    Player player;
    int random;

    public void SetEntites(Player player, Monster monster)
    {
        this.player = player;
        this.monster = monster;
    }

    public void UseSkill(int index)
    {
        switch (index)
        {
            case 1: AttackSpeedUp(); break;
            default: break;
        }
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

    public void CriticalAttack()
    {
        random = Random.Range(0, 10);
        if (random == 9)
        {
            monster.ATK *= 2f;
            Debug.Log("치명타 터짐");
        }
    }
}
