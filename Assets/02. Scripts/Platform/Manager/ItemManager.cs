using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<int, Items> allItemDataBase; // 전체 아이템 데이터들
    public Dictionary<int, Items> holdItemDataBase; // 들고 있는 아이템들
    public Dictionary<int, Items> wearItemDataBase; // 입고 있는 아이템들
    
    private InvenUI invenUI;
    private bool canUseItem = true;
    private WaitForSeconds itemCoolTime = new WaitForSeconds(0.2f);

    void Start()
    {
        SetUpItemDictionary();
    }

    private void SetUpItemDictionary()
    {
        allItemDataBase = Managers.Data.ItemDict;
        holdItemDataBase = Managers.Data.ItemDict;
        //holdItemDataBase = new Dictionary<int, Items>();
    }

    public void GetItem(int _itemID, int _itemCnt = 1)
    {
        if (holdItemDataBase.ContainsKey(_itemID))
            holdItemDataBase[_itemID].itemCnt += _itemCnt;
        else
        {
            holdItemDataBase.Add(_itemID, holdItemDataBase[_itemID]);
            holdItemDataBase[_itemID].itemCnt += _itemCnt - 1;
        }
    }

    public void StartUseItem(int _itemID, int _itemCnt=1)
    {
        StartCoroutine(UseItem(_itemID, _itemCnt));
    }

    IEnumerator UseItem(int _itemID, int _itemCnt = 1)
    {
        if (holdItemDataBase.ContainsKey(_itemID) && canUseItem)
        {
            holdItemDataBase[_itemID].itemCnt -= _itemCnt;
            ItemEffect(_itemID);
            DeleteItem(_itemID);
            canUseItem = false;
            yield return itemCoolTime;
            canUseItem = true;
        }
    }

    public void ItemEffect(int _itemID, int _sign=1)
    {
        switch (holdItemDataBase[_itemID].itemEffect)
        {
            case "plusHP":
                PlayerStat.instance.HealPlayer(holdItemDataBase[_itemID].itemFigure* _sign);
                break;
            case "maxHP":
                PlayerStat.instance.ChangeStat(holdItemDataBase[_itemID].itemFigure * _sign,0,0,0);
                break;
            case "attackPower":
                PlayerStat.instance.ChangeStat(0,holdItemDataBase[_itemID].itemFigure * _sign,0,0);
                break;
            case "maxCost":
                PlayerStat.instance.ChangeStat(0,0,holdItemDataBase[_itemID].itemFigure * _sign,0);
                break;
            case "costRecoverySpeed":
                PlayerStat.instance.ChangeStat(0,0,0,holdItemDataBase[_itemID].itemFigure * _sign);
                break;
            default:
                break;
        }
    }

    public void DeleteItem(int _itemID)
    {
        if (holdItemDataBase[_itemID].itemCnt <= 0)
        {
            holdItemDataBase.Remove(_itemID);
            invenUI.ClassificationItems();
        }
            
    }

    public void TakeOnEquipment()
    {

    }

    public void TakeOffEquipment(int _itemID)
    {
        ItemEffect(_itemID, -1);
    }

    public void RegisterInvenUI(InvenUI _invenUI)
    {
        invenUI = _invenUI;
    }

}
