using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] GameObject introduceBox;
    [SerializeField] GameObject merchantUI;

    private bool isOpenMerchantUI = false;
    private List<ItemData>sellList = new List<ItemData>();
    
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
