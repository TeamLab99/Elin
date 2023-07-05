using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : Singleton<StoreUI>
{
    [SerializeField] Text aumText;
    [SerializeField] Transform displayList;
    [SerializeField] DisplaySlot[] displaySlots;
    [SerializeField] GameObject storeObject;
    public GameObject decisionBuy;

    private int aum;
    private Items clickObject = null;
    private bool canClick = true;

    private void Awake()
    {
        displaySlots = displayList.GetComponentsInChildren<DisplaySlot>();
        SetAum();
    }

    public void SetAum()
    {
        aum = ItemManager.instance.LoadAum();
        aumText.text = aum.ToString();
    }

    public void ShowSellItems(List <Items> _sellList)
    {
        for(int i=0; i< _sellList.Count; i++)
        {
            displaySlots[i].AddItem(_sellList[i]);
        }
        for(int i=_sellList.Count; i<displaySlots.Length; i++)
        {
            displaySlots[i].DeleteItem();
        }
    }

    public void ClearSellItems()
    {
        for (int i = 0; i < displaySlots.Length; i++)
        {
            displaySlots[i].DeleteItem();
        }
    }

    public void ClickExitStoreBtn()
    {
        if (canClick)
        {
            ClearSellItems();
            storeObject.SetActive(false);
        }
    }

    public void OnDecisionBuy(Items _item)
    {
        decisionBuy.SetActive(true);
        for(int i=0; i < displaySlots.Length; i++)
        {
            displaySlots[i].ControlBtnClick(false);
            canClick = false;
        }
        clickObject = _item;
    }

    public void OffDecisionBuy()
    {
        for (int i = 0; i < displaySlots.Length; i++)
        {
            displaySlots[i].ControlBtnClick(true);
            canClick = true;
        }
        decisionBuy.SetActive(false);
    }

    public bool CanBuyObject()
    {
        if (aum >= clickObject.buyPrice)
        {
            ItemManager.instance.UseAum(clickObject.buyPrice);
            ItemManager.instance.GetItem(clickObject.itemID);
            return true;
        }
        else
        {
            return false;
        }
    }
}
