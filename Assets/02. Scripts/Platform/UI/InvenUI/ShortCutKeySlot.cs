using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShortCutKeySlot : MonoBehaviour
{
    public int keyBtn;
    [SerializeField] ShortCutKeySlot[] allShortKeySlots;
    [SerializeField] Text itemCnt;
    [SerializeField] Image itemIcon;
    Items holdItemInfo;
    int itemIdx;
    bool canUse;
    Color nonItemColor = Color.white;
    Color holdItemColor = Color.white;

    private void Awake()
    {
        nonItemColor.a = 0f;
        holdItemColor.a = 1f;
    }

    public void ChangeItemInfo(Items _itemDatas, int _itemIdx)
    {
        itemIcon.color = holdItemColor;
        itemIcon.sprite = _itemDatas.itemIcon;
        itemCnt.text = _itemDatas.itemCnt.ToString();
        holdItemInfo = _itemDatas;
        itemIdx = _itemIdx;
        canUse = true;
    }


    private void Update()
    {
        if (canUse)
        {
            if (keyBtn == 1 && Input.GetKeyDown(KeyCode.Alpha1))
            {
                ClickUse();
                UpdateCnt();
            }
            if (keyBtn == 2 && Input.GetKeyDown(KeyCode.Alpha2))
            {
                ClickUse();
                UpdateCnt();
            }
            if (keyBtn == 3 && Input.GetKeyDown(KeyCode.Alpha3))
            {
                ClickUse();
                UpdateCnt();
            }
        }
    }

    public void ClickUse()
    {
        if (holdItemInfo == null)
        {
            return;
        }
        if (holdItemInfo.itemCnt == 1)
        {
            ItemManager.instance.UseItem(holdItemInfo.itemID);
            itemCnt.text = holdItemInfo.itemCnt.ToString();
            ResetShortCutKeySlot();
            canUse = false;
        }
        else
        {
            ItemManager.instance.UseItem(holdItemInfo.itemID);
            itemCnt.text = holdItemInfo.itemCnt.ToString();
        }
    }

    public void UpdateCnt()
    {
        for(int i=0; i<allShortKeySlots.Length; i++)
        {
            allShortKeySlots[i].UpdateCntText();
        }
        
    }

    public void UpdateCntText()
    {
        if (holdItemInfo != null)
            itemCnt.text = holdItemInfo.itemCnt.ToString();
    }

    public void ResetShortCutKeySlot()
    {
        itemIcon.color = nonItemColor;
        itemIcon.sprite = null;
        holdItemInfo = null;
        itemCnt.text = " ";
        itemIdx = -1;
    }
}
