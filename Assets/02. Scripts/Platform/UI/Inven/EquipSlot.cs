using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    private Button button;
    private int itemID;
    private Items holdItemInfo;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ItemManager.instance.TakeOffEquipment(holdItemInfo));
    }

    public void AddItem(Items _itemData)
    {
        itemIcon.sprite = _itemData.itemIcon;
        holdItemInfo = _itemData;
    }

    public void RemoveItem()
    {
        itemIcon.sprite = null;
    }
}
