using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item_Data 
{
    public int itemID; //아이템 고유번호(유일)
    public string itemName; // 아이템 이름 (중복 가능)
    public string itemDescription; // 아이템 설명
    public int itemCount;
    public Sprite itemIcon;
    public ItemType itemType;
    public enum ItemType //아이템 분류
    {
          Consumer,
          Equipment,
          Quest
    }
    public Item_Data(int _itemID, string _itemName ,string _itemDescription,ItemType _itemType, int _itemCount= 1)
    {
        itemID = _itemID;
        itemCount = _itemCount;
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemType = _itemType;
        itemIcon = Resources.Load("Main_Page/Sprites/Items/" + itemID.ToString(), typeof(Sprite)) as Sprite; //Sprite로 가져왔지만 변환이 안되서 as Sprite를 붙어야 한다.
    } // 아이템 정보
    
}
