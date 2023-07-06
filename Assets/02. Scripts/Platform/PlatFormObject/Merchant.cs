using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] int[] sellItemsID;
    [SerializeField] GameObject introduceBox;
    [SerializeField] GameObject storeObject;

    private bool isOpenMerchantUI = false;
    public bool isQuestClear = false;
    public List<Items>sellList = new List<Items>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            introduceBox.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKey(KeyCode.X) && !isOpenMerchantUI && isQuestClear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SetSellItemList();
                storeObject.SetActive(true);
                isOpenMerchantUI = true;
                introduceBox.SetActive(false);
            }      
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOpenMerchantUI = false;
            storeObject.SetActive(false);
            StoreUI.instance.ClearSellItems();
            introduceBox.SetActive(false);
        }
    }
}
