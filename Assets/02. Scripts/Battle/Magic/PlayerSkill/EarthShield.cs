using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthShield : BuffDebuffMagic, IMagic
{
    private void Awake()
    {
        Managers.Data.CardDict.TryGetValue(4, out card);

        probability = card.attackProbability;
        mainTime = card.buffMaintainTime;
        amount = MagicManager.instance.player.GetLoseHealth();
    }

    public override IEnumerator Timer()
    {
        while (time > 0)
        {
            yield return oneSec;

            if (!isTimerStop)
                time -= 1f;

            icon.coolTimeImage.fillAmount -= 0.1f;
        }
        isEnd = true;
        Delete();
    }

    public override void Delete()
    {
        StopCoroutine(Timer());
        if (TryGetComponent<Poolable>(out var poolable))
            Managers.Pool.Push(poolable);
        BuffIconsController.instance.DeleteIconInfo(icon);
        buffManager.buffDebuffList.Remove(this);
    }

    private void OnDisable()
    {
        probability = card.attackProbability;
        mainTime = card.buffMaintainTime;
        amount = 0;

        isEnd = false;
        isTimerStop = false;
        time = mainTime;
    }

    void IMagic.DeSpell()
    {
        Delete();
    }

    void IMagic.Spell()
    {
        // 몬스터의 공격력 amount 만큼 차감

        // 삭제
        Delete();
    }

    public override void IconInit()
    {
        base.IconInit();
        icon.amountText.text = MagicManager.instance.player.GetLoseHealth().ToString();
    }
}