using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase_Manager : MonoBehaviour
{
    public List<Item_Data> itemList = new List<Item_Data>();
    static public DataBase_Manager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    } // 아이템 데이터 매니저는 항상 존재

    void Start() 
    {
        itemList.Add(new Item_Data(10001, "cherry", "체력을 10 증가", Item_Data.ItemType.Consumer));
        itemList.Add(new Item_Data(10002, "gem", "체력을 30 증가", Item_Data.ItemType.Consumer));
        itemList.Add(new Item_Data(20001, "bag", "방어력 10 증가", Item_Data.ItemType.Equipment));
        itemList.Add(new Item_Data(20002, "belt", "방어력 30 증가", Item_Data.ItemType.Equipment));
        itemList.Add(new Item_Data(30001, "bone", "해골몬스터의 뼈조각", Item_Data.ItemType.Quest));
        itemList.Add(new Item_Data(30002, "egg", "닭 몬스터의 알", Item_Data.ItemType.Quest));
        itemList.Add(new Item_Data(40001, "silk", "방어구의 재료", Item_Data.ItemType.Etc));
        itemList.Add(new Item_Data(40010, "diamond", "무기의 재료", Item_Data.ItemType.Etc));
    } // 아이템 정보를 입력

  
    void Update()
    {
        
    }
}