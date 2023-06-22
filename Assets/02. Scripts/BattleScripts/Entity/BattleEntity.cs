using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class BattleEntity : MonoBehaviour
{
    [SerializeField] TMP_Text hpTMP;

    float hp;
    float maxHp;
    float defense;
    float attack;
    float maxAttack;
    float buffDefense;
    bool attackable;
    
    //Buff[] buffs;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public abstract void Init();
    public abstract void BuffCheck();

    protected void HpTextUpdate()
    {
        hpTMP.text = hp.ToString();
    }

    protected void Attack(BattleEntity entity)
    {
        entity.TakeDamage(attack);
    }

    protected void TakeDamage(float value)
    {
        if (hp - value > 0)
        {
            hp -= value;
        }
        else
        {
            hp = 0;
        }
        HpTextUpdate();
    }
}
