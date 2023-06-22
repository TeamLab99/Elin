using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    [SerializeField] Image itemIcon=null;
    private Button button;
    private int itemID;
    private Items holdItemInfo;
    Color transparentColor = Color.white;
    Color nontransparentColor = Color.white;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(RemoveItem);
        transparentColor.a = 0f;
        nontransparentColor.a = 1f;
    }

    public void AddItem(Items _itemData)
    {
        itemIcon.color = nontransparentColor;
        itemIcon.sprite = _itemData.itemIcon;
        holdItemInfo = _itemData;
    }

    public void RemoveItem()
    {
        if (holdItemInfo == null)
            return;
        itemIcon.sprite = null;
        itemIcon.color = transparentColor;
        ItemManager.instance.TakeOffEquipment(holdItemInfo);
        holdItemInfo = null;
    }
}
