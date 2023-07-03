using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplaySlot : MonoBehaviour
{

    [SerializeField] Text priceText;
    [SerializeField] Image displayItemIcon;
    [SerializeField] Button button;

    Items displayItemInfo;
    Color transparetnColor = Color.clear;
    Color nonTransparentColor = Color.white;

    private void Awake()
    {
        button.onClick.AddListener(() => StoreUI.instance.OnDecisionBuy(displayItemInfo));
    }

    public void AddItem(Items _itemInfo)
    {
        displayItemInfo = _itemInfo;
        displayItemIcon.color = nonTransparentColor;
        displayItemIcon.sprite = _itemInfo.itemIcon;
        priceText.text = _itemInfo.buyPrice.ToString();
    }

    public void DeleteItem()
    {
        displayItemIcon.color = transparetnColor;
        displayItemInfo = null;
        priceText.text = " ";
    }

    public void ControlBtnClick(bool _control)
    {
        button.interactable = _control;
    }
}
