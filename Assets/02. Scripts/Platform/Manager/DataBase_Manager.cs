using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase_Manager : MonoBehaviour
{
    public List<Item_Data> itemList = new List<Item_Data>();
    void Start() 
    {
        itemList.Add(new Item_Data(10002, "gem", "마나를 10 증가", Item_Data.ItemType.Consumer,0,0,0,10));
        itemList.Add(new Item_Data(20001, "bag", "방어력 10 증가", Item_Data.ItemType.Equipment,0,10));
        itemList.Add(new Item_Data(20002, "belt", "공격력 10 증가", Item_Data.ItemType.Equipment,10));
        itemList.Add(new Item_Data(30001, "bone", "해골몬스터의 뼈조각", Item_Data.ItemType.Quest,0));
        itemList.Add(new Item_Data(30002, "egg", "닭 몬스터의 알", Item_Data.ItemType.Quest,0));
    } 
}
