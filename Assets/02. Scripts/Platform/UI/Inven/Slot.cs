using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    private Text itemCntText;
    private Button button;
    private int itemID;
    private void Awake()
    {
        button = GetComponent<Button>();
        itemCntText = GetComponentInChildren<Text>();
        button.onClick.AddListener(()=> ItemManager.instance.StartUseItem(itemID));
    }

    public void AddItem(Items _itemData)
    {
        itemIcon.sprite = _itemData.itemIcon;
        itemID = _itemData.itemID;
        if (_itemData.itemType != "Equ")
            itemCntText.text = "X " + _itemData.itemCnt.ToString();
        else
            itemCntText.text = " ";
    }

    public void RemoveItem()
    {
        itemCntText.text = " ";
        itemIcon.sprite = null;
    } 

}
