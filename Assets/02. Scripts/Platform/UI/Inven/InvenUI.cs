using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Con,
    Equ,
    Etc
}

public class InvenUI : MonoBehaviour
{
    public string itemTypeString;
    public ItemTypes itemTypeEnum = ItemTypes.Con;
   
    private int slotSize=0;
    private Slot[] itemSlots;
    private Dictionary<int, Items> invenItems; // 인벤토리에 저장된 아이템
    private List<Items> showItemList = new List<Items>(); // 보여지는 아이템들
    private Dictionary<int, Items> wearEquipDic = new Dictionary<int, Items>(); // 착용한 장비 아이템

    private void Start()
    {
        itemSlots = GetComponentsInChildren<Slot>();
    }

    public void ClassificationItems(ItemTypes _itemType)
    {
        showItemList.Clear();
        itemTypeEnum = _itemType;
        itemTypeString = itemTypeEnum.ToString();
        foreach (KeyValuePair<int,Items> itemList in ItemManager.instance.holdItemDataBase)
        {
            if (itemList.Value.itemType == itemTypeString)
                showItemList.Add(itemList.Value);
        }
        ShowItem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ClassificationItems(itemTypeEnum);
        }
    }
    public void ShowItem()
    {
        slotSize = showItemList.Count;
        for (int i = 0; i < slotSize; i++)
        {
            itemSlots[i].gameObject.SetActive(true);
            itemSlots[i].AddItem(showItemList[i]);
        }
        for(int i=slotSize; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveItem();
            itemSlots[i].gameObject.SetActive(false);
        }
    }
}
