using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] Text itemCntText;
    private Button button;
    private Items holdItemInfo;
    private int itemIdx;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickButton);
    }

    public void AddItem(Items _itemData, int _itemIdx)
    {
        itemIcon.sprite = _itemData.itemIcon;
        holdItemInfo = _itemData;
        itemIdx = _itemIdx;        
        if (_itemData.itemType != "Equ")
            itemCntText.text = "X " + _itemData.itemCnt.ToString();
        else
            itemCntText.text = " ";
    }

    public void RemoveItem()
    {
        itemCntText.text = " ";
        itemIcon.sprite = null;
    } 

    public void ClickButton()
    {
        switch (holdItemInfo.itemID / 100)
        {
            case 2:
                ItemManager.instance.TakeOnEquipment(holdItemInfo, itemIdx);
                break;
            default:
                ItemManager.instance.UseItem(holdItemInfo.itemID);
                break;
        }
    }
}
