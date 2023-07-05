using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePlayer : BattleEntity
{
    private void Start()
    {
        hp = maxHp;
        Init();
    }

    public override void Init()
    {
        battleBuffDebuff = gameObject.AddComponent<BattleBuffManager>();
        StartCoroutine(GetGaugeUI("HpBar"));
    }

    public override IEnumerator GetGaugeUI(string tagName)
    {
        yield return new WaitForEndOfFrame();
        hpBar = GameObject.FindGameObjectsWithTag(tagName)[0].GetComponent<Image>();
        hpTMP = GameObject.FindGameObjectsWithTag("HpText")[0].GetComponent<TMP_Text>();
        HpTextUpdate();
    }

    public override void TimerControl(bool isStop)
    {
        throw new System.NotImplementedException();
    }

    public void MagicAttack(BattleEntity entity, float value)
    {
        entity.TakeDamage(value);
    }
}
