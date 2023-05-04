using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataBase_Manager : MonoBehaviour
{
    public List<Item_Data> itemList = new List<Item_Data>();
    static public DataBase_Manager instance;
    public Pool_Manager pool;
    public Player_Move pm;
    public GameObject status;
    public Spawn_Manager spm;
    public Camera_Follow cm;
    private bool isOpen = false;

    //public TextAsset itemdata;
    //private AllItemData datas;

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
      //  datas = JsonUtility.FromJson<AllItemData>(itemdata.text);
    } // 아이템 데이터 매니저는 항상 존재

    void Start() 
    {
        //Debug.Log(datas.itemDatas[0].itemID);
        //itemList.Add(new Item_Data(datas.itemDatas[0].itemID, datas.itemDatas[0].itemName, datas.itemDatas[0].itemDescription, datas.itemDatas[0].itemType, 0,0,10));
        itemList.Add(new Item_Data(10002, "gem", "마나를 10 증가", Item_Data.ItemType.Consumer,0,0,0,10));
        itemList.Add(new Item_Data(20001, "bag", "방어력 10 증가", Item_Data.ItemType.Equipment,0,10));
        itemList.Add(new Item_Data(20002, "belt", "공격력 10 증가", Item_Data.ItemType.Equipment,10));
        itemList.Add(new Item_Data(30001, "bone", "해골몬스터의 뼈조각", Item_Data.ItemType.Quest,0));
        itemList.Add(new Item_Data(30002, "egg", "닭 몬스터의 알", Item_Data.ItemType.Quest,0));
    } // 아이템 정보를 입력

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isOpen)
                isOpen = false;
            else
                isOpen = true;
            status.SetActive(isOpen);
        }*/

        //if (Input.GetKeyDown(KeyCode.P))
            //Player_Stat.instance.TakeDamage();
    }
}
