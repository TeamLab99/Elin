using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angry_Monster : BuffDebuffMagic
{
    public override IEnumerator Timer()
    {
        yield return null;
    }

    public override void IconInit()
    {
        //icon.countText.text = time.ToString();
        icon.coolTimeImage.fillAmount = 0f;
        icon.IconImage.sprite = skilIcon;
        icon.isFull = true;
        icon.buff = this;

        TextUpdate();
    }

    public override void Delete()
    {
        StopCoroutine(Timer());
        Managers.Pool.Push(GetComponent<Poolable>());
        BuffIconsController.instance.DeleteIconInfo(icon);
        buffManager.buffDebuffList.Remove(this);
    }

    public void TextUpdate()
    {
        if (amount < 2)
        {
            amount++;
            icon.amountText.text = amount.ToString();
        }
    }
}
