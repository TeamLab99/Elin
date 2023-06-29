using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public abstract class BattleEntity : MonoBehaviour
{
    [SerializeField] TMP_Text hpTMP;
    [SerializeField] protected float maxHp;
    [SerializeField] public float attack;

    public BattleBuffManager battleBuffDebuff;
    protected float hp;

    public abstract void Init();
    public abstract void TimerControl(bool isStop);

    private void Start()
    {
        BattleCardManager.EffectPlayBack += TimerControl;
        hp = maxHp;
        Init();
        HpTextUpdate();
    }

    private void OnDestroy()
    {
        BattleCardManager.EffectPlayBack -= TimerControl;
    }

    protected void HpTextUpdate()
    {
        hpTMP.text = hp.ToString();
    }

    public virtual void Attack(BattleEntity entity)
    {
        entity.TakeDamage(attack);
    }

    public void TakeDamage(float value)
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

    public void Heal(int value)
    {
        if (hp + value > maxHp)
        {
            hp = maxHp;
        }
        else
        {
            hp += value;
        }
        HpTextUpdate();
    }
}
