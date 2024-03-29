using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wet : BuffDebuffMagic, IMagic
{
    public int count =0;

    public bool GetCount()
    {
        if (count < 2)
        {
            count += 1;
            return false;
        }
        else
        {
            count = 0;
            return true;
        }
    }
    
    private void Awake()
    {
        mainTime = 15f;
    }

    public override IEnumerator Timer()
    {
        while (time > 0)
        {
            yield return oneSec;

            if (!isTimerStop)
            {
                time -= 1f;
                icon.coolTimeImage.fillAmount -= 0.067f;
            }
        }
        isEnd = true;
        Delete();
    }

    public override void Delete()
    {
        StopCoroutine("Timer");
        BuffIconsController.instance.DeleteIconInfo(icon);
        buffManager.buffDebuffList.Remove(this);

        if (TryGetComponent<Poolable>(out var poolable))
            Managers.Pool.Push(poolable);
    }

    private void OnDisable()
    {
        mainTime = 15f;
        amount = 0;

        isEnd = false;
        isTimerStop = false;
        time = mainTime;
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
