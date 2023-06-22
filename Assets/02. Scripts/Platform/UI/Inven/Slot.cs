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
    Color transparentColor = Color.white;
    Color nontransparentColor = Color.white;
    
    private int itemIdx;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickButton);
        transparentColor.a = 0f;
        nontransparentColor.a = 1f;
    }

    public void AddItem(Items _itemData, int _itemIdx)
    {
        itemIcon.color = nontransparentColor;
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
        transparentColor.a = 0f;
        itemIcon.color = transparentColor;
    } 

    public void ClickButton()
    {
        if (holdItemInfo == null)
        {
            return;
        }
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
