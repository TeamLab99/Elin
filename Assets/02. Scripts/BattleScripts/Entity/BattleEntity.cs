using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public abstract class BattleEntity : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] public float attack;
    protected TMP_Text hpTMP;

    public BattleBuffManager battleBuffDebuff;
    protected float hp;
    protected Image hpBar;

    public abstract void Init();
    public abstract void TimerControl(bool isStop);

    public virtual IEnumerator GetGaugeUI(string tagName)
    {
        yield return new WaitForEndOfFrame();
        HpTextUpdate();
    }

    private void Start()
    {
        BattleCardManager.EffectPlayBack += TimerControl;
        hp = maxHp;
        Init();
    }

    private void OnDestroy()
    {
        BattleCardManager.EffectPlayBack -= TimerControl;
    }

    protected void HpTextUpdate()
    {
        hpTMP.text = hp.ToString() + " / " + maxHp.ToString();

        hpBar.fillAmount = hp / maxHp;
    }

    public virtual void Attack(BattleEntity entity)
    {
        entity.TakeDamage(attack);
    }

    public virtual void TakeDamage(float value)
    {
        if (hp - value > 0)
        {
            hp -= value;
        }
        else
        {
            hp = 0;
            BattleGameManager.instance.GameOver();
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
