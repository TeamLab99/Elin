using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Inst { get; private set; }
    void Awake() => Inst = this;

    Monster monster;
    Player player;

    public void SetEntites(Player player, Monster monster)
    {
        this.player = player;
        this.monster = monster;
    }

    public void UseMagic(int index)
    {
        switch (index)
        {
            case 1: NormalAtk(); break;
            case 2: Heal(); break;
            default: break;
        }
    }

    public void NormalAtk()
    {
        player.Attack(monster);
    }

    public void Heal()
    {
        player.Heal(5);
    }
}
