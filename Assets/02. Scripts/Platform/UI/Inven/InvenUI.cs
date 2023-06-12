using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemTypes
{
    Con,
    Equ,
    Etc
}

public class InvenUI : MonoBehaviour
{
    public Slot[] itemSlots;
    public Text moneyText;
    public Transform itemSlotsTransform;
    public EquipSlot[] equipSlots;
    public Transform equipSlotsTransform;
    public ItemTypes itemTypeEnum = ItemTypes.Con;

    private int slotSize=0;
    public int wearSlotSize = 0;
    private string itemTypeString;
    private InvenTabSlot[] invenTabSlots;
    private List<Items> showItemList = new List<Items>(); // 보여지는 아이템들
    private List<Items> showWearList = new List<Items>(); // 착용한 아이템들

    private void Awake()
    {
        itemSlots = itemSlotsTransform.GetComponentsInChildren<Slot>();
        equipSlots = equipSlotsTransform.GetComponentsInChildren<EquipSlot>();
        invenTabSlots = GetComponentsInChildren<InvenTabSlot>();
        ItemManager.instance.RegisterInvenUI(this);
    }

    public void ClassificationItems()
    {
        showItemList.Clear();
        itemTypeString = itemTypeEnum.ToString();
        switch (itemTypeEnum)
        {
            case ItemTypes.Equ:
                for(int i=0; i< ItemManager.instance.holdEquipList.Count; i++)
                {
                    showItemList.Add(ItemManager.instance.holdEquipList[i]);
                }
                break;
            default:
                foreach (KeyValuePair<int, Items> itemList in ItemManager.instance.holdItemDataBase)
                {
                    if (itemList.Value.itemType == itemTypeString)
                        showItemList.Add(itemList.Value);
                }
                break;
        }
        ShowItem();
    }

    public void ShowItem()
    {
        slotSize = showItemList.Count;
        for (int i = 0; i < slotSize; i++)
        {
            itemSlots[i].gameObject.SetActive(true);
            itemSlots[i].AddItem(showItemList[i],i);
        }
        for(int i=slotSize; i < itemSlots.Length; i++)
        {
            itemSlots[i].RemoveItem();
            itemSlots[i].gameObject.SetActive(false);
        }
    }


    public void WearEquipmentItems()
    {
        showWearList = ItemManager.instance.wearEquipList;
        ShowEquipment();
    }

    public void ShowEquipment()
    {
        wearSlotSize = showWearList.Count;
        for (int i = 0; i < wearSlotSize; i++)
        {
            equipSlots[i].gameObject.SetActive(true);
            equipSlots[i].AddItem(showWearList[i]);
        }
        for (int i = wearSlotSize; i <4; i++)
        {
            equipSlots[i].RemoveItem();
            equipSlots[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        ClassificationItems();
        WearEquipmentItems();
        SetUpMoneyText();
        //StartCoroutine("LoadingItemList");
    }

    public void ChangeTab(ItemTypes _itemType)
    {
        itemTypeEnum = _itemType;
        ClassificationItems();
        for (int i = 0; i < invenTabSlots.Length; i++)
            invenTabSlots[i].SelectButtonEffect();
    }

    /*IEnumerator LoadingItemList()
    {
        yield return new WaitForEndOfFrame();
    }*/

    public void SetUpMoneyText()
    {
        moneyText.text = ItemManager.instance.GetMoneyInfo().ToString();
    }
}
