using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePlayer : BattleEntity
{
    
    bool scriptOnce = false;
    private int count;
    
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

    public void SetStat(PlayerStatData data)
    {
        hp = data.currentHP;
        maxHp = data.maxHP;
        attack = data.attackPower;

        BattleCardManager.instance.SetStat(data.maxCost, data.costRecoverySpeed);
    }
    
    public virtual void TakeDamage(float value)
    {
        if (hp - value < 11)
        {
            //넘을 수 없는 벽 시작
            BattleCardManager.instance.DontUseCard(true);
            MobSkillManager.instance.MonsterAttackSpeedDown();
        }
        else if (hp - value > 10)
        {
            hp -= value;
        }
        else if (hp - value <= 0)
        {
            hp = 0;
            // 죽음
        }
        HpTextUpdate();
    }
    
}
