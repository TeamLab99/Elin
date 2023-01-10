using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Slot : MonoBehaviour
{
    public Image icon;
    public GameObject selected_Item;
    public Text itemName_Text;
    public Text itemCount_Text;
    
    public void AddItem(Item_Data _item) 
    {
        itemName_Text.text = _item.itemName;
        icon.sprite = _item.itemIcon;
        if (Item_Data.ItemType.Consumer == _item.itemType) // 아이템 개수를 출력시킴
        {
            if (_item.itemCount > 0)
                itemCount_Text.text = "X " + _item.itemCount.ToString();
            else
                itemCount_Text.text = " "; 
        } 
    } // Item_Data 스크립트에서 item 정보를 받아오고 비교하여 슬롯에 추가
    public void RemoveItem()
    {
        itemCount_Text.text = " ";
        itemName_Text.text = " ";
        icon.sprite = null;
    } // 아이템을 소모하면 삭제 
}
