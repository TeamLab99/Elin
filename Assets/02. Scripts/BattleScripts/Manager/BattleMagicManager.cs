using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMagicManager : Singleton<BattleMagicManager>
{
    BattlePlayer player;
    BattleMonster monster;

    [SerializeField] MagicSO magic;

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
        var skillEffect = magic.items[card.index - 1].skillEffect;
        var skillIcon = magic.items[card.index - 1].skillIcon;
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
        var skillEffect = magic.items[card.index - 1].skillEffect;
        var skillIcon = magic.items[card.index - 1].skillIcon;
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

            var magic = effect.GetComponent<BuffDebuffMagic>();
            playerBuffDebuffList.Add(magic);
            magic.ConnectBuffManager(player.battleBuffDebuff, BuffIconsController.instance.GetBuffIconInfo(true));
        }

    }

    #endregion
}