using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class MobSkillManager : Singleton<MobSkillManager>
{
    BattlePlayer player;
    BattleMonster monster;

    [SerializeField] MonsterSO monsterSO;
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);


    public void SetEntites(BattlePlayer player, BattleMonster monster)
    {
        this.player = player;
        this.monster = monster;
    }

    public IEnumerator CallNormalAttackEffect(int index)
    {
        var normalAttack = monsterSO.items[index-1].normalAttackEffect;

        var effect = Managers.Pool.Pop(normalAttack, player.transform.Find("PlayerEffects"));
        effect.transform.position = player.gameObject.transform.position;

        yield return delay05;
        Managers.Pool.Push(effect);
    }

    public void CallSkill(int index, float probability)
    {
        switch (index)
        {
            case 1:
                Broadening();
                break;
            default:
                Debug.Log("존재하지 않는 .");
                break;
        }
    }

    public void Broadening()
    {

    }

    public bool GetRandom(float probability)
    {
        float percentage = probability / 100;
        float rate = 100 - (100 * percentage);
        int tmp = (int)Random.Range(0, 100);

        if (tmp <= rate - 1)
        {
            return false;
        }
        return true;
    }
}