using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float maxHp;

    public float MaxHp => maxHp;

    public float Hp => hp;

    [SerializeField] public float attack;
    protected TMP_Text hpTMP;

    [FormerlySerializedAs("battleBuffDebuff")] public BuffManager buffDebuff;
    protected float hp;
    protected Image hpBar;
    protected float healMainTainTime;
    private Coroutine heal;

    public abstract void Init();
    public abstract void TimerControl(bool isStop);

    public virtual IEnumerator GetGaugeUI(string tagName)
    {
        yield return new WaitForEndOfFrame();
        HpTextUpdate();
    }

    protected void HpTextUpdate()
    {
        hpTMP.text = hp.ToString() + " / " + maxHp.ToString();

        hpBar.fillAmount = hp / maxHp;
    }

    public virtual void Attack(Entity entity)
    {
        entity.TakeDamage(attack);
    }

    public virtual void TakeDamage(float value) { }

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
    
    public IEnumerator SustainedHeal(float time)
    {
        healMainTainTime = time;
        
        while (healMainTainTime > 0)
        {
            yield return new WaitForSeconds(1f);
            healMainTainTime -= 1f;
            Heal(5);
        }
        
        healMainTainTime = 0;
    }
    
    public void HealReset(float time)
    {
        if(heal != null)
        {
            StopCoroutine(heal);

            healMainTainTime = 0;
            
            heal = StartCoroutine(SustainedHeal(time));
        }
        else
        {
            heal = StartCoroutine(SustainedHeal(time));
        }
    }
}
