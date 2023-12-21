using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Entity
{
    private int count;

    private void Start()
    {
        hp = maxHp;
        Init();
    }

    public override void Init()
    {
        buffDebuff = gameObject.AddComponent<BuffManager>();
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

    public void MagicAttack(Entity entity, float value)
    {
        entity.TakeDamage(value);
    }

    public void SetStat(PlayerStatData data)
    {
        hp = data.currentHP;
        maxHp = data.maxHP;
        attack = data.attackPower;

        CardManager.instance.SetStat(data.maxCost, data.costRecoverySpeed);
    }

    public override void TakeDamage(float value)
    {
        if (hp - value < 11)
        {
            //넘을 수 없는 벽 시작
            if((MagicManager.instance.monster as Nightmare).GetIsWall() == false)
                PlatformEventManager.instance.NextEricaDialogue();
            
            CardManager.instance.DontUseCard(true);
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
            MagicManager.instance.monster.EntitiesStateChange(true);
            Time.timeScale = 0;
            PlatformEventManager.instance.NextEricaDialogue();
        }

        HpTextUpdate();
    }

    public int GetLoseHealth()
    {
        return (int)((maxHp - hp));
    }
}