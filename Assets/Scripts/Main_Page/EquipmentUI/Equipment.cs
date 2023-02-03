using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    private const int WEAPON = 0, SHILED=1, AMULT=2, LEFTRING=3, RIGHTRING=4, HELMET=5, ARMOR=6, LEFTGLOVE=7, RIGHTGLOVE=8,
        BELT=9,LEFTBOOT=10, RIGHTBOOT=11;
    private PlayerStat theplayerStat; //스탯 불러오기
    private Inventory theInven; // 인벤토리 불러오기
    public GameObject go;
    public Text[] text; // 스탯
    public Image[] imgSlots; // 장비 슬롯 아이콘
    public GameObject goselectedSlotUI; // 선택된 장비 슬롯UI
    public Item_Data[] equipItemList; // 장착된 장비 리스트
    private int selectedSlot; // 선택된 장비 슬롯
    public bool activated;
    private bool inputKey=true;
    private void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theplayerStat = FindObjectOfType<PlayerStat>();
    }

    public void ClearEquipment()
    {
        Color color = imgSlots[0].color;
        color.a = 0f;
        for(int i=0; i < imgSlots.Length; i++)
        {
            imgSlots[i].sprite = null;
            imgSlots[i].color = color;
        }
    }

    public void ShowEquipment()
    {
        Color color = imgSlots[0].color;
        color.a = 1f;
        for (int i = 0; i < imgSlots.Length; i++)
        {
            if (equipItemList[i].itemID != 0)
            {
                imgSlots[i].sprite = equipItemList[i].itemIcon;
                imgSlots[i].color = color;
            }
        }
    }
    public void SelectedSlot()
    {
        goselectedSlotUI.transform.position = imgSlots[selectedSlot].transform.position;
    }
    public void TakeOffEquip()
    {
       // theInven.EquipToInventory(equipItemList[selectedSlot]);
        equipItemList[selectedSlot] = new Item_Data(0, "", "", Item_Data.ItemType.Equipment);
        ClearEquipment();
        ShowEquipment();
    }
    public void EquipItem(Item_Data _item)
    {
        string temp = _item.itemID.ToString();
        temp = temp.Substring(0, 3);
        switch (temp)
        {
            case "200": // 무기
                EquipItemCheck(WEAPON, _item);
                break;
            case "201": // 방패
                EquipItemCheck(SHILED, _item);
                break;
            case "202": // 아뮬렛
                EquipItemCheck(AMULT, _item);
                break;
            case "203": // 반지
                EquipItemCheck(LEFTRING, _item);
                break;

        }
    }
    public void EquipItemCheck(int _count, Item_Data _item)
    {
        if (equipItemList[_count].itemID == 0)
        {
            equipItemList[_count] = _item;
        }
        else
        {
          //  theInven.EquipToInventory(equipItemList[_count]);
            equipItemList[_count] = _item;
        }
    }
    
    private void Update()
    {
        if (inputKey)
        {
            if (activated)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedSlot < imgSlots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot < imgSlots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = imgSlots.Length - 1;
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = imgSlots.Length-1;
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    inputKey = false;
                    if (equipItemList[selectedSlot].itemID != 0)
                        TakeOffEquip();
                    else
                        EquipItem(equipItemList[selectedSlot]);
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                activated = !activated;
                if (activated)
                {
                    go.SetActive(true);
                    selectedSlot = 0;
                    ClearEquipment();
                    ShowEquipment();
                }
                else
                {
                    go.SetActive(false);
                    ClearEquipment();
                }
            }
        }
    }
}
