using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : BuffDebuffMagic, IMagic
{
    private void Awake()
    {
        Managers.Data.CardDict.TryGetValue(1, out card);

        probability = card.attackProbability;
        maintime = card.buffMaintainTime;
        amount = card.amount;
    }

    public override IEnumerator Timer()
    {
        while (time > 0)
        {
            yield return oneSec;

            if (!isTimerStop)
                time -= 1f;

            icon.coolTimeImage.fillAmount -= 0.33f;
        }
        isEnd = true;
        Delete();
    }

    public override void Delete()
    {
        StopCoroutine(Timer());
        BuffIconsController.instance.DeleteIconInfo(icon);
        buffManager.buffDebuffList.Remove(this);

        if (TryGetComponent<Poolable>(out var poolable))
            Managers.Pool.Push(poolable);
    }

    private void OnDisable()
    {
        probability = card.attackProbability;
        maintime = card.buffMaintainTime;
        amount = card.amount;

        isEnd = false;
        isTimerStop = false;
        time = maintime;
    }

    void IMagic.DeSpell()
    {
        // 존재하는 버프 삭제 하는 기믹이 있을 때 사용.
        Delete();
    }

    void IMagic.Spell()
    {
        // 버프 창에 있다가 특정 조건에 의해 발동하는 카드를 위해 사용

        // 삭제
        Delete();
    }
}
