using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreListSlot : MonoBehaviour
{
    Button button;
    StoreUI storeUI;
    Items displayItemInfo;
    List<Items> buyList = new List<Items>();
    [SerializeField] Image displayItemIcon;
    [SerializeField] Text itemCntText;

    private void Awake()
    {
        button = GetComponent<Button>();
        storeUI = GetComponentInParent<StoreUI>();
       // button.onClick.AddListener(() => );
    }

    public void AddItem(Items _itemInfo)
    {
        displayItemInfo = _itemInfo;
        displayItemIcon.sprite = _itemInfo.itemIcon;
        itemCntText.text = _itemInfo.itemCnt.ToString();
    }


}
