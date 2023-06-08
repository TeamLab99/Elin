using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<int, Items> itemDictionary;
    public Dictionary<int,DeckCard > tempDic;
    void SetUpItemDictionary()
    {
        itemDictionary = Managers.Data.ItemDict;
    }

    void Start()
    {
        SetUpItemDictionary();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach(int k in itemDictionary.Keys)
            {
                Debug.Log(k);
            }
        }
    }
}
