using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] int[] sellItemsID;
    [SerializeField] GameObject introduceBox;
    [SerializeField] GameObject merchantUI;
    StoreUI storeUI;

    private bool isOpenMerchantUI = false;
    public List<Items>sellList = new List<Items>();

    private void Awake()
    {
        storeUI = merchantUI.GetComponent<StoreUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            introduceBox.SetActive(true);
    }

    public void SetSellItemList()
    {
        sellList.Clear();
        for(int i=0; i < sellItemsID.Length; i++)
        {
            sellList.Add(ItemManager.instance.allItemDataBase[sellItemsID[i]]);
        }
        storeUI.ShowSellItems(sellList);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKey(KeyCode.X) && !isOpenMerchantUI)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SetSellItemList();
                isOpenMerchantUI = true;
                merchantUI.SetActive(true);
                introduceBox.SetActive(false);
            }      
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOpenMerchantUI = false;
            merchantUI.SetActive(false);
            introduceBox.SetActive(false);
        }
    }
}
