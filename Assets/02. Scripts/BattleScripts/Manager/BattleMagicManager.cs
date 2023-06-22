using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMagicManager : Singleton<BattleMagicManager>
{
    Player player;
    BattleMonster monster;

    [SerializeField]GameObject skillPrefab;
    [SerializeField]GameObject skillEffectPrefab;
    GameObject hitEffectPrefab;

    GameObject skill;
    public void SetEntites(Player player, BattleMonster monster)
    {
        this.player = player;
        this.monster = monster;

        //Managers.Pool.CreatePool(skillPrefab);
    }

/*    public void CallMagic(DeckCard magic)
    {
        switch (magic.type)
        {
            case "A":
                Attack(magic);
                break;
            case "B":
                Buff(magic);
                break;
            case "D":
                Debuff(magic);
                break;
            case "AB":
                AttackAndBuff(magic);
                break;
            case "AD":
                AttackAndDebuff(magic);
                break;
            case "BD":
                BuffAndDebuff(magic);
                break;
            default:
                Debug.Log("존재하지 않는 타입의 마법입니다.");
                break;
        }
    }*/

}