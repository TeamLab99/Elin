using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_Slot : MonoBehaviour
{
    public Image icon;
    public void AddItem(Item_Data _item)
    {
        icon.sprite = _item.itemIcon;
        Debug.Log(_item.itemName);
    } // Item_Data 스크립트에서 item 정보를 받아오고 비교하여 슬롯에 추가
    public void RemoveItem()
    {
        icon.sprite = null;
    } // 아이템을 소모하면 삭제 
}
