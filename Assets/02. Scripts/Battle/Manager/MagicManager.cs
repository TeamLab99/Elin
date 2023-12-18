using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using System.Linq;
using Yarn.Compiler;

public class MagicManager : Singleton<MagicManager>
{
    public Player player;
    public Monster monster;
    [SerializeField] Camera mainCamera;

    [SerializeField] MagicSO magic;

    WaitForSeconds delay05 = new WaitForSeconds(0.5f);

    public void SetEntites(Player player, Monster monster)
    {
        this.player = player;
        this.monster = monster;
    }

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }

    public void CallMagic(DeckCard card)
    {
        switch (card.cardName)
        {
            case "구르기":
                Rolling(card);
                break;
            case "박치기":
                StartCoroutine(HeadButt(card));
                break;
            case "재생":
                Heal(card);
                break;
            case "버티기":
                Defense(card);
                break;
            case "물거품":
                StartCoroutine(Bubble(card));
                break;
            case "불티":
                StartCoroutine(Spark(card));
                break;
            case "물초":
                StartCoroutine(WaterWeed(card));
                break;
            case "물대포":
                StartCoroutine(WaterStream(card));
                break;
            case "여우비":
                FoxRain(card);
                break;
            case "낫족제비":
                StartCoroutine(Wind(card));
                break;
            case "울림":
                StartCoroutine(EarthQuake(card));
                break;
            default:
                Debug.Log("존재하지 않는 마법입니다.");
                break;
        }
    }

    #region Buff

    public void Rolling(DeckCard card)
    {
        if (!GetRandom(card.buffProbability))
            return;

        var skillEffect = magic.items[card.index - 1].skillEffect;
        var playerBuffDebuffList = player.buffDebuff.buffDebuffList;

        if (playerBuffDebuffList.Find(item => item is Rolling))
        {
            var skill = playerBuffDebuffList.Find(item => item is Rolling);
            skill.TimeUpdate();
        }
        else
        {
            var effect = Managers.Pool.Pop(skillEffect, player.transform.Find("PlayerEffects"));
            effect.transform.position = player.gameObject.transform.position;

            var magic = effect.GetComponent<BuffDebuffMagic>();
            playerBuffDebuffList.Add(magic);
            magic.ConnectBuffManager(player.buffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
        }
    }

    public void Defense(DeckCard card)
    {
        if (!GetRandom(card.buffProbability))
            return;

        var skillEffect = magic.items[card.index - 1].skillEffect;
        var playerBuffDebuffList = player.buffDebuff.buffDebuffList;

        if (playerBuffDebuffList.Find(item => item is Defense))
        {
            var skill = playerBuffDebuffList.Find(item => item is Defense);
            skill.TimeUpdate();
        }
        else
        {
            var effect = Managers.Pool.Pop(skillEffect, player.transform.Find("PlayerEffects"));
            effect.transform.position = player.gameObject.transform.position;

            var buff = effect.GetComponent<BuffDebuffMagic>();
            playerBuffDebuffList.Add(buff);
            buff.ConnectBuffManager(player.buffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
        }
    }

    public void Heal(DeckCard card)
    {
        if (!GetRandom(card.buffProbability))
            return;

        var skillEffect = magic.items[card.index - 1].skillEffect;

        var effect = Managers.Pool.Pop(skillEffect, player.transform.Find("PlayerEffects"));
        effect.transform.position = player.gameObject.transform.position;

        player.Heal(card.amount);
    }

    #endregion

    public IEnumerator HeadButt(DeckCard card)
    {
        if (!GetRandom(card.attackProbability))
            yield break;

        var skillEffect = magic.items[card.index - 1].skillEffect;

        var effect = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
        effect.transform.position = monster.gameObject.transform.position + Vector3.up * 0.5f;

        player.transform.DOMoveX(5f, 0.3f).SetRelative().SetEase(Ease.Flash, 2, 0);
        mainCamera.DOShakePosition(0.3f, 2);
        player.MagicAttack(monster, card.amount);
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(effect);
        monster.ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator Bubble(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        startEfc.transform.position = player.gameObject.transform.position + Vector3.up * 0.5f;

        skillEffect = magic.items[card.index - 1].hitEffect;

        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position + Vector3.up * 0.5f;

        player.MagicAttack(monster, card.amount);

        if (GetRandom(card.debuffProbability))
        {
            WetBuff(card);
        }

        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }

    public void WetBuff(DeckCard card)
    {
        var buff = magic.items[card.index - 1].buffEffect;
        var playerBuffList = player.buffDebuff.buffDebuffList;

        var drowning = playerBuffList.Find(item => item is Drowning);
        
        if (drowning != null)
        {
            return;
        }
        
        var skill = playerBuffList.Find(item => item is Wet);
            
        if (skill != null)
        {
            if((skill as Wet).GetCount() == true)
            {
                // 익수
                playerBuffList.Remove(skill);
                skill.Delete();

                buff = magic.items[6].buffEffect;
                var effect = Managers.Pool.Pop(buff, monster.transform.Find("MobEffects"));
                effect.transform.position = monster.gameObject.transform.position;
                
                var drown = effect.GetComponent<BuffDebuffMagic>();
                playerBuffList.Add(drown);
                drown.ConnectBuffManager(monster.buffDebuff, BuffIconsController.instance.GetBuffIconInfo(false));

                monster.DownSpeedReset(20, 15);
                monster.DrowningReset(20);
            }
            else
            {
                skill.TimeUpdate();
                
                monster.DownSpeedReset(15, (skill as Wet).count * 5);
            }
            
        }
        else
        {
            var effect = Managers.Pool.Pop(buff, player.transform.Find("MobEffects"));
            effect.transform.position = monster.gameObject.transform.position;

            var wet = effect.GetComponent<BuffDebuffMagic>();
            playerBuffList.Add(wet);
            wet.ConnectBuffManager(monster.buffDebuff, BuffIconsController.instance.GetBuffIconInfo(false));
        }
    }

    public void DrowningReset()
    {
        var drowning = player.buffDebuff.buffDebuffList.Find(item => item is Drowning);
        if (drowning != null)
        {
            drowning.TimeUpdate();
            
            monster.DownSpeedReset(20,15);
            monster.DrowningReset(20);
        }
    }
    

    public IEnumerator Spark(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        startEfc.transform.position = player.gameObject.transform.position;

        skillEffect = magic.items[card.index - 1].hitEffect;
        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position;

        monster.ChangeAnim(EMonsterState.Hit);

        player.MagicAttack(monster, card.amount);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator WaterStream(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        startEfc.transform.position = player.gameObject.transform.position;

        skillEffect = magic.items[card.index - 1].hitEffect;

        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position;

        player.MagicAttack(monster, card.amount);
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator Wind(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        startEfc.transform.position = player.gameObject.transform.position;

        skillEffect = magic.items[card.index - 1].hitEffect;

        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position;

        player.MagicAttack(monster, card.amount);
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator EarthQuake(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        startEfc.transform.position = player.gameObject.transform.position;

        skillEffect = magic.items[card.index - 1].hitEffect;

        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position;

        player.MagicAttack(monster, card.amount);
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator WaterWeed(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        startEfc.transform.position = monster.gameObject.transform.position;

        player.MagicAttack(monster, card.amount);
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }

    public void FoxRain(DeckCard card)
    {
        // if (!GetRandom(card.buffProbability))
        //     return;

        var skillEffect = magic.items[card.index - 1].skillEffect;

        var effect = Managers.Pool.Pop(skillEffect, player.transform.Find("PlayerEffects"));
        effect.transform.position = player.gameObject.transform.position;
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

    public void ClearBuff()
    {
        GameObject.FindGameObjectsWithTag("Skill").ToList()
            .ForEach(item => Managers.Pool.Push(item.GetComponent<Poolable>()));
        player.buffDebuff.ClearBuff();
        monster.buffDebuff.ClearBuff();
    }
}