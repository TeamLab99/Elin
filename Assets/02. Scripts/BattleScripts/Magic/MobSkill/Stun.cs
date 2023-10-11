using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : BuffDebuffMagic, IMagic
{
    private void Awake()
    {
        mainTime = 2f;
        time = mainTime;
    }
    
    public override IEnumerator Timer()
    {

        while (time > 0)
        {
            yield return oneSec;

            if (!isTimerStop)
            {
                time -= 1f;
                
                if(icon!=null)
                    icon.coolTimeImage.fillAmount -= 0.5f;
            }

        }
        isEnd = true;
        Delete();
    }
    
    private void OnDisable()
    {
        mainTime = 2f;
        isEnd = false;
        isTimerStop = false;
        time = mainTime;
    }
    
    public override void Delete()
    {
        StopCoroutine(Timer());
        BuffIconsController.instance.DeleteIconInfo(icon);
        
        if(buffManager!=null)
            buffManager.buffDebuffList.Remove(this);

        if (TryGetComponent<Poolable>(out var poolable))
            Managers.Pool.Push(poolable);
    }

    public void Spell()
    {
        throw new System.NotImplementedException();
    }

    public void DeSpell()
    {
        throw new System.NotImplementedException();
    }
}
