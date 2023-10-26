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
            effect.transform.position = player.gameObject.transform.position + Vector3.up * 2f;
        }
        else
        {
            var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
            effect.transform.position = player.gameObject.transform.position + Vector3.up * 2f;

            var buff = effect.GetComponent<BuffDebuffMagic>();
            playerbuffList.Add(buff);
            buff.ConnectBuffManager(player.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
        }
        yield return delay05;
    }

    public IEnumerator Valley()
    {
        var skillEffect = monsterSO.items[4].skillEffect[0];
        var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));

        effect.transform.position = monster.gameObject.transform.position;

        yield return delay05;
    }
    
    public IEnumerator Rush()
    {
        var skillEffect = Managers.Pool.Pop(monsterSO.items[2].skillEffect[0], monster.transform.Find("MobEffects"));
        var hitEffect = Managers.Pool.Pop(monsterSO.items[2].normalAttackEffect, monster.transform.Find("MobEffects"));
        var playerbuffList = player.battleBuffDebuff.buffDebuffList;
        
        skillEffect.transform.position = monster.gameObject.transform.position;
        hitEffect.transform.position = player.gameObject.transform.position;

        monster.AttackValue(player, 10f);
        var stunTime = 2f;

        if (Utils.RandomPercent(30) == true)
        {
            StartCoroutine(monster.GetComponent<Nightmare>().StunPlayer(stunTime));

            if (playerbuffList.Find(item => item is Stun))
            {
                var skill = playerbuffList.Find(item => item is Stun);
                ((Stun)skill).TimeUpdate();

            }
            else
            {
                var buff = skillEffect.GetComponent<BuffDebuffMagic>();
                playerbuffList.Add(buff);
                buff.ConnectBuffManager(player.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
            }
        }

        yield return delay05;
    }
    
    public IEnumerator Drain()
    {
        var skillEffect = monsterSO.items[5].skillEffect[0];
        var monsterBuffList = monster.battleBuffDebuff.buffDebuffList;

        if (monsterBuffList.Find(item => item is Drain))
        {
            var skill = monsterBuffList.Find(item => item is Drain);
            
            ((Drain)skill).TextUpdate(10);

            var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
            effect.transform.position = monster.gameObject.transform.position;
            
            if (((Drain)skill).GetAmount() == true)
            {
                monster.GetComponent<Nightmare>().DrainHeal();
                monster.GetComponent<Nightmare>().SetIsDrain(false);
                ((Drain)skill).Delete();
            }
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

    public void MonsterAttackSpeedDown()
    {
        monster.GetComponent<Nightmare>().AttackSpeedDown();
    }
}