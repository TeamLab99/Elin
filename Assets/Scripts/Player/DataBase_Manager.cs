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
    }

    void Start()
    {
        itemList.Add(new Item_Data(10001, "cherry", "체력을 10 증가", Item_Data.ItemType.Consumer));
        itemList.Add(new Item_Data(10002, "gem", "체력을 30 증가", Item_Data.ItemType.Consumer));
    }

  
    void Update()
    {
        
    }
}
