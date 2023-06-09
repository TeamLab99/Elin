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
    public Slot[] itemSlots;
    public Transform itemSlotsTransform;
    public ItemTypes itemTypeEnum = ItemTypes.Con;
   
    private int slotSize=0;
    private string itemTypeString;
    private InvenTabSlot[] invenTabSlots;
    private Dictionary<int, Items> invenItems; // 인벤토리에 저장된 아이템
    private List<Items> showItemList = new List<Items>(); // 보여지는 아이템들
    private Dictionary<int, Items> wearEquipDic = new Dictionary<int, Items>(); // 착용한 장비 아이템


    private void Awake()
    {
        itemSlots = itemSlotsTransform.GetComponentsInChildren<Slot>();
        invenTabSlots = GetComponentsInChildren<InvenTabSlot>();
        ItemManager.instance.RegisterInvenUI(this);
    }

    public void ClassificationItems()
    {
        showItemList.Clear();
        itemTypeString = itemTypeEnum.ToString();
        foreach (KeyValuePair<int,Items> itemList in ItemManager.instance.holdItemDataBase)
        {
            if (itemList.Value.itemType == itemTypeString)
                showItemList.Add(itemList.Value);
        }
        ShowItem();
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


    public void ChangeTab(ItemTypes _itemType)
    {
        itemTypeEnum = _itemType;
        ClassificationItems();
        for(int i=0; i<invenTabSlots.Length; i++)
            invenTabSlots[i].SelectButtonEffect();
    }

    private void OnEnable()
    {
        ClassificationItems();
        //StartCoroutine("LoadingItemList");
    }

    /*IEnumerator LoadingItemList()
    {
        yield return new WaitForEndOfFrame();
    }*/
}
