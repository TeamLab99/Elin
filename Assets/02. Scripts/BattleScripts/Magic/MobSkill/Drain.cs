using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drain : BuffDebuffMagic
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

        TextUpdate(1);
    }

    public override void Delete()
    {
        StopCoroutine(Timer());
        if (TryGetComponent<Poolable>(out var poolable))
            Managers.Pool.Push(poolable);
        BuffIconsController.instance.DeleteIconInfo(icon);
        buffManager.buffDebuffList.Remove(this);
        amount = 0;
    }

    public void TextUpdate(int count)
    {
        if (amount == 0)
        {
            amount = 1;
            icon.amountText.text = amount.ToString();

        }
        else if (amount < count)
        {
            amount++;
            icon.amountText.text = amount.ToString();
        }
    }

    public bool GetAmount()
    {
        return amount == 10;
    }
}
