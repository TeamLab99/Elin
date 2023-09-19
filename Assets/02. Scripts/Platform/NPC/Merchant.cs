using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] int[] sellItemsID;
    [SerializeField] GameObject introduceBox;
    [SerializeField] GameObject storeObject;
   
    public List<Items>sellList = new List<Items>();
    private bool isEnter = false;

    private void Update()
    {
        if(isEnter && Input.GetKeyDown(KeyCode.X))
        {
            SetSellItemList();
            StoreUI.instance.SetAum();
            storeObject.SetActive(true);
            Managers.Input.PlayerMoveControl(false);
        }
    }
    public void SetSellItemList()
    {
        sellList.Clear();
        for (int i = 0; i < sellItemsID.Length; i++)
        {
            sellList.Add(ItemManager.instance.allItemDataBase[sellItemsID[i]]);
        }
        StoreUI.instance.ShowSellItems(sellList);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isEnter = true;
            introduceBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isEnter = false;
            introduceBox.SetActive(false);
        }
    }
}
