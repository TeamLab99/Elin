using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BattleMagicManager : Singleton<BattleMagicManager>
{
    BattlePlayer player;
    BattleMonster monster;

    [SerializeField] MagicSO magic;

    WaitForSeconds delay05 = new WaitForSeconds(0.5f);

    public void SetEntites(BattlePlayer player, BattleMonster monster)
    {
        this.player = player;
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
        player.MagicAttack(monster, card.amount);

        yield return delay05;
        Managers.Pool.Push(effect);
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