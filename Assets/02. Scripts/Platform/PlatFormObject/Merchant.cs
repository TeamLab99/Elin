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
    public Dictionary<int, Items>sellList = new Dictionary<int, Items>();

    private void Awake()
    {
        storeUI = merchantUI.GetComponent<StoreUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            introduceBox.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKey(KeyCode.X) && !isOpenMerchantUI)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                sellList = ItemManager.instance.allItemDataBase;
                storeUI.ShowSellItems(sellList);
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
