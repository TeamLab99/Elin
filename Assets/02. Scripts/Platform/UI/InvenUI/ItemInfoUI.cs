using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    [SerializeField] Text itemDescription;
    [SerializeField] Text itemEffect;
    [SerializeField] Text itemName;
    [SerializeField] Image itemIcon;
    Items holdItemInfo;
    int itemIdx;

    public void ChangeItemInfo(Items _itemDatas, int _itemIdx)
    {
        itemDescription.text = _itemDatas.itemDescription;
        itemEffect.text =  "플레이어의 체력을 10 올려줍니다.";
        itemName.text = _itemDatas.itemName;
        itemIcon.sprite = _itemDatas.itemIcon;
        holdItemInfo = _itemDatas;
        itemIdx = _itemIdx;
    }

    public void ClickUse()
    {
        if (holdItemInfo == null)
        {
            return;
        }
        switch (holdItemInfo.itemID / 100)
        {
            case 2:
                ItemManager.instance.TakeOnEquipment(holdItemInfo, itemIdx);
                ResetItemInfo();
                break;
            default:
                DialogueManager.instance.ClearQuest("Erica");
                if (holdItemInfo.itemCnt == 1)
                {
                    ItemManager.instance.UseItem(holdItemInfo.itemID);
                    ResetItemInfo();
                }else
                    ItemManager.instance.UseItem(holdItemInfo.itemID);
                break;
        }
    }

    public void ResetItemInfo()
    {
        itemDescription.text = null;
        itemEffect.text = null;
        itemName.text = null;
        itemIcon.sprite = null;
        holdItemInfo = null;
        itemIdx = -1;
        gameObject.SetActive(false);
    }
}
