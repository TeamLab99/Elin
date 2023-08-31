using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<int, Items> allItemDataBase; // 전체 아이템 데이터들
    public Dictionary<int, Items> holdItemDataBase; // 들고 있는 아이템들
    public List<Items> holdEquipList;
    public List<Items> wearEquipList;
    private int Aum = 0;
    private WaitForSeconds itemCoolTime = new WaitForSeconds(0.2f);

    void Start()
    {
        SetUpItemDataBase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            EarnAum();
        }
    }

    private void SetUpItemDataBase()
    {
        allItemDataBase = Managers.Data.ItemDict;
        holdItemDataBase = new Dictionary<int, Items>();
        holdEquipList = new List<Items>();
        wearEquipList = new List<Items>();
    }

    public void GetItem(int _itemID, int _itemCnt = 1)
    {
        if (_itemID / 100 != 2)  // 소모품, 기타 아이템
        {
            if (holdItemDataBase.ContainsKey(_itemID))
                holdItemDataBase[_itemID].itemCnt += _itemCnt;
            else
            {
                holdItemDataBase.Add(_itemID, allItemDataBase[_itemID]);
                holdItemDataBase[_itemID].itemCnt = _itemCnt;
            }   
        }
        else // 장비 아이템
            holdEquipList.Add(allItemDataBase[_itemID]);
        InvenUI.instance.ClassificationItems();
    }

    public void UseItem(int _itemID, int _itemCnt=1) // 기타, 소모품만 사용
    {
        if (_itemID / 100 != 2)
        {
            if (holdItemDataBase.ContainsKey(_itemID))
            {
                holdItemDataBase[_itemID].itemCnt -= _itemCnt;
                UseItemEffect(_itemID);
                DeleteItem(_itemID);
            }
        }
    }

    public void UseItemEffect(int _itemID, int _sign=1)
    {
        switch (holdItemDataBase[_itemID].itemEffect)
        {
            case "plusHP":
                PlayerStatManager.instance.HealPlayer(holdItemDataBase[_itemID].itemFigure* _sign);
                break;
            default:
                break;
        }
    }

    public void DeleteItem(int _itemID)
    {
        if (holdItemDataBase[_itemID].itemCnt <= 0)
            holdItemDataBase.Remove(_itemID);
        InvenUI.instance.ClassificationItems();
    }

    public void TakeOnEquipment(Items _equipItem, int _itemIdx)
    {
        if (InvenUI.instance.wearSlotSize < 4)
        {
            holdEquipList.RemoveAt(_itemIdx);
            wearEquipList.Add(_equipItem);
            EquipItemEffect(_equipItem);
            InvenUI.instance.ClassificationItems();
            InvenUI.instance.WearEquipmentItems();
        }
    }

    public void TakeOffEquipment(Items _equipItem)
    {
        holdEquipList.Add(_equipItem);
        wearEquipList.Remove(_equipItem);
        EquipItemEffect(_equipItem, -1);
        InvenUI.instance.ClassificationItems();
        InvenUI.instance.WearEquipmentItems();
    }

    public void EquipItemEffect(Items _equipEffect, int _sign = 1)
    {
        switch (_equipEffect.itemEffect)
        {
            case "maxHP":
                PlayerStatManager.instance.ChangeStat(_equipEffect.itemFigure * _sign, 0, 0, 0);
                break;
            case "attackPower":
                PlayerStatManager.instance.ChangeStat(0, _equipEffect.itemFigure * _sign, 0, 0);
                break;
            case "maxCost":
                PlayerStatManager.instance.ChangeStat(0, 0, _equipEffect.itemFigure * _sign, 0);
                break;
            case "costRecoverySpeed":
                PlayerStatManager.instance.ChangeStat(0, 0, 0, _equipEffect.itemFigure * _sign);
                break;
            default:
                break;
        }
    }

    public int LoadAum()
    {
        return Aum;
    }

    public void UseAum(int _use)
    {
        Aum -= _use;
        InvenUI.instance.SetAum();
        StoreUI.instance.SetAum();
    }

    public void EarnAum(int _earn=1000)
    {
        Aum += _earn;
        InvenUI.instance.SetAum();
        StoreUI.instance.SetAum();
    }

    public void Clear()
    {

    }
}
