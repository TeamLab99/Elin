using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : BuffDebuffMagic, IMagic
{
    private void Awake()
    {
        Managers.Data.CardDict.TryGetValue(4, out card);

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

            icon.countText.text = time.ToString();
        }
        isEnd = true;
        Delete();
    }

    public override void Delete()
    {
        StopCoroutine(Timer());
        Managers.Pool.Push(GetComponent<Poolable>());
        BuffIconsController.instance.DeleteIconInfo(icon);
        buffManager.buffDebuffList.Remove(this);
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
        Delete();
    }

    void IMagic.Spell()
    {
        // 몬스터의 공격력 amount 만큼 차감

        // 삭제
        Delete();
    }
}
