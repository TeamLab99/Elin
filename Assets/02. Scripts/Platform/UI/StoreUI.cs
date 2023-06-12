using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    List<Items> sellList = new List<Items>();
    List<Items> buyList = new List<Items>();
    [SerializeField] Text allBuyPriceText;
    [SerializeField] Text allSellPriceText;
    [SerializeField] Transform displaySellList;
    [SerializeField] Transform displayBuyList;
    [SerializeField] DisplaySlot[] displaySlots;
    StoreListSlot[] storeListSlots;
    int allBuyPrice = 0;
    int allSellPrice = 0;
    // Update is called once per frame

    private void Awake()
    {
        storeListSlots = displayBuyList.GetComponentsInChildren<StoreListSlot>();
        // buyBtns = buyList.GetComponentsInChildren<Button>();
    }

    public void ShowSellItems(List <Items> _sellList)
    {
        sellList.Clear();
        for(int i=0; i< _sellList.Count; i++)
        {
            displaySlots[i].AddItem(_sellList[i]);
            sellList.Add(_sellList[i]);
        }
        for(int i=_sellList.Count; i<displaySlots.Length; i++)
        {
            displaySlots[i].DeleteItem();
        }
    }

    public void AddBuyList(Items _item)
    {
        if (buyList.Count <= 6)
        {
            for(int i=0; i<buyList.Count; i++)
            {
                if(_item.itemID == buyList[i].itemID)
                {
                    buyList[i].itemCnt += 1;
                    storeListSlots[i].AddItem(buyList[i]);
                    allBuyPrice += buyList[i].sellPrice;
                    allBuyPriceText.text = allBuyPrice.ToString();
                    return;
                }
            }
            if (buyList.Count == 6)
                return;
            buyList.Add(_item);
            storeListSlots[buyList.Count-1].AddItem(buyList[buyList.Count - 1]);
            allBuyPrice += buyList[buyList.Count-1].sellPrice;
            allBuyPriceText.text = allBuyPrice.ToString();
        }
    }
}
