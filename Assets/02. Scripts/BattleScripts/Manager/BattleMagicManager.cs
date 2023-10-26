using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using System.Linq;

public class BattleMagicManager : Singleton<BattleMagicManager>
{
    public BattlePlayer player;
    public BattleMonster monster;
    [SerializeField]Camera mainCamera;

    [SerializeField] MagicSO magic;

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
        var playerBuffDebuffList = player.battleBuffDebuff.buffDebuffList;

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
            magic.ConnectBuffManager(player.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
        }
    }

    public void Defense(DeckCard card)
    {
        if (!GetRandom(card.buffProbability))
            return;

        var skillEffect = magic.items[card.index - 1].skillEffect;
        var playerBuffDebuffList = player.battleBuffDebuff.buffDebuffList;

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
            buff.ConnectBuffManager(player.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
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
        effect.transform.position = monster.gameObject.transform.position + Vector3.up*0.5f;

        player.transform.DOMoveX(5f, 0.3f).SetRelative().SetEase(Ease.Flash, 2, 0);
        mainCamera.DOShakePosition(0.3f,2);
        player.MagicAttack(monster, card.amount);
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(effect);
        monster.ChangeAnim(EMonsterState.Idle);
    }
    
    public IEnumerator Bubble(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
        startEfc.transform.position = player.gameObject.transform.position + Vector3.up*0.5f;
        
        skillEffect = magic.items[card.index - 1].hitEffect;
        
        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position + Vector3.up*0.5f;

        if (GetRandom(card.debuffProbability))
        {
            var buff = magic.items[card.index - 1].buffEffect;
            var playerBuffDebuffList = player.battleBuffDebuff.buffDebuffList;

            if (playerBuffDebuffList.Find(item => item is Wet))
            {
                var skill = playerBuffDebuffList.Find(item => item is Wet);
                skill.TimeUpdate();
            }
            else
            {
                var effect = Managers.Pool.Pop(buff, player.transform.Find("PlayerEffects"));
                effect.transform.position = player.gameObject.transform.position;

                var magic = effect.GetComponent<BuffDebuffMagic>();
                playerBuffDebuffList.Add(magic);
                magic.ConnectBuffManager(player.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
            }
        }



        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }
    
    public IEnumerator Spark(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
        startEfc.transform.position = player.gameObject.transform.position;
        
        skillEffect = magic.items[card.index - 1].hitEffect;
        
        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position;
        
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }
    
    public IEnumerator WaterStream(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
        startEfc.transform.position = player.gameObject.transform.position;
        
        skillEffect = magic.items[card.index - 1].hitEffect;
        
        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position;
        
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }
    
    public IEnumerator Wind(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
        startEfc.transform.position = player.gameObject.transform.position;
        
        skillEffect = magic.items[card.index - 1].hitEffect;
        
        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position;
        ;
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }
    
    public IEnumerator EarthQuake(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
        startEfc.transform.position = player.gameObject.transform.position;
        
        skillEffect = magic.items[card.index - 1].hitEffect;
        
        var hitEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("PlayerEffects"));
        hitEfc.transform.position = monster.gameObject.transform.position;
        
        monster.ChangeAnim(EMonsterState.Hit);

        yield return delay05;
        Managers.Pool.Push(startEfc);
        monster.ChangeAnim(EMonsterState.Idle);
    }
    
    public IEnumerator WaterWeed(DeckCard card)
    {
        var skillEffect = magic.items[card.index - 1].skillEffect;

        var startEfc = Managers.Pool.Pop(skillEffect, monster.transform.Find("MobEffects"));
        startEfc.transform.position = monster.gameObject.transform.position;
        
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
        GameObject.FindGameObjectsWithTag("Skill").ToList().ForEach(item => Managers.Pool.Push(item.GetComponent<Poolable>()));
        player.battleBuffDebuff.ClearBuff();
        monster.battleBuffDebuff.ClearBuff();
    }


}