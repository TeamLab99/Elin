using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplaySlot : MonoBehaviour
{
    Items displayItemInfo;
    Button button;
    StoreUI storeUI;
    [SerializeField] Image displayItemIcon;

    private void Awake()
    {
        button = GetComponent<Button>();
        storeUI = GetComponentInParent<StoreUI>();
        button.onClick.AddListener(()=> storeUI.AddBuyList(displayItemInfo));
    }

    public void AddItem(Items _itemInfo)
    {
        displayItemInfo = _itemInfo;
        displayItemIcon.sprite = _itemInfo.itemIcon;
    }
}
