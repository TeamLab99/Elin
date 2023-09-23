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

    public void SetMonster(BattleMonster monster)
    {
        this.monster = monster;
    }

    public IEnumerator CallNormalAttackEffect(int index)
    {
        var normalAttack = monsterSO.items[index - 1].normalAttackEffect;

        var effect = Managers.Pool.Pop(normalAttack, player.transform.Find("PlayerEffects"));
        effect.transform.position = player.gameObject.transform.position;

        yield return delay05;
        Managers.Pool.Push(effect);
    }

    public void CallSkill(int index, float probability = 100)
    {
        switch (index)
        {
            case 1:
                StartCoroutine(Broadening());
                break;
            case 2:
                StartCoroutine(Broadening_NightMare());
                break;
            default:
                Debug.Log("존재하지 않는 .");
                break;
        }
    }

    public IEnumerator Broadening()
    {
        var skillEffect = monsterSO.items[0].skillEffect[0];
        var monsterBuffList = monster.battleBuffDebuff.buffDebuffList;

        if (monsterBuffList.Find(item => item is Angry_Monster))
        {
            var skill = monsterBuffList.Find(item => item is Angry_Monster);
            ((Angry_Monster)skill).TextUpdate(2);

            var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
            effect.transform.position = monster.gameObject.transform.position;
        }
        else
        {
            var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
            effect.transform.position = monster.gameObject.transform.position;

            var buff = effect.GetComponent<BuffDebuffMagic>();
            monsterBuffList.Add(buff);
            buff.ConnectBuffManager(monster.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(false));
        }
        yield return delay05;
    }

    public IEnumerator Broadening_NightMare()
    {
        var skillEffect = monsterSO.items[0].skillEffect[0];
        var monsterBuffList = monster.battleBuffDebuff.buffDebuffList;

        if (monsterBuffList.Find(item => item is Angry_Monster))
        {
            var skill = monsterBuffList.Find(item => item is Angry_Monster);
            ((Angry_Monster)skill).TextUpdate(4);

            var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
            effect.transform.position = monster.gameObject.transform.position;
        }
        else
        {
            var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
            effect.transform.position = monster.gameObject.transform.position;

            var buff = effect.GetComponent<BuffDebuffMagic>();
            monsterBuffList.Add(buff);
            buff.ConnectBuffManager(monster.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(false));
        }
        yield return delay05;
    }

    public IEnumerator Fear()
    {
        var skillEffect = monsterSO.items[3].skillEffect[0];
        var playerbuffList = player.battleBuffDebuff.buffDebuffList;

        if (playerbuffList.Find(item => item is Fear))
        {
            var skill = playerbuffList.Find(item => item is Fear);
            ((Fear)skill).TextUpdate();

            var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
            effect.transform.position = player.gameObject.transform.position;
        }
        else
        {
            var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
            effect.transform.position = player.gameObject.transform.position;

            var buff = effect.GetComponent<BuffDebuffMagic>();
            playerbuffList.Add(buff);
            buff.ConnectBuffManager(player.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
        }
        yield return delay05;
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