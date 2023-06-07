using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private DataBase_Manager theDatabase;
    private Inventory_Slot[] slots; // 인벤토리 슬롯들
    private Equipment_Slot[] eslots;
    private List<Item_Data> inventoryItemList; //플레이어가 소지한 아이템 리스트
    private List<Item_Data> inventoryTabList; // 선택한 템에 따라 다르게 보여질 아이템 리스트
    private List<Item_Data> equipmentTakeOnList; // 착용한 장비템 리스트 
    public Text description_Text; // 부연설명
    public Text itemName_Text; // 아이템 이름
    public Text use_Text;
    public Text drop_Text;
    public Transform tf; // 슬롯의 부모객체
    public Transform etf;
    public GameObject useBtnOnOff;
    public Image icon;

    public Button[] invenBtn;
    public Button[] tabBtn;
    public Button[] equipBtn;
    public Button[] useBtn;
    
    public GameObject go; //인벤토리 활성화 비활성하
    public GameObject[] selectedTabImages;
    
    //public GameObject selectionWindow;
    private int selectedItem=0; //선택된 아이템
    private int selectedTab=0; //선택된 탭
    private int selectedEquip=0;
    private bool activated; //인벤토리 활성화시 true
    private bool canUse=false;
    
    private bool stopKeyInput; // 키 입력 제한 (소비할 때 나오는 질의 중에)
    private bool showEquipStat=false; // 착용한 장비의 스탯을 보여주는지
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    void Start()
    {
        instance = this;
        theDatabase = FindObjectOfType<DataBase_Manager>(); // 한번 접근이 아니라 여러번 접근이므로 find사용
        inventoryItemList = new List<Item_Data>();
        inventoryTabList = new List<Item_Data>();
        equipmentTakeOnList = new List<Item_Data>();
        slots = tf.GetComponentsInChildren<Inventory_Slot>();
        eslots = etf.GetComponentsInChildren<Equipment_Slot>();
        OnClickButton();
    }

    void OnClickButton() // 버튼 클릭 시, 클릭한 번호의 순서를 반환해주는 함수를 불러온다. (버튼들에 함수를 연결한다.)
    {
        for (int i = 0; i < invenBtn.Length; i++)
        {
            int temp = i;
            invenBtn[i].onClick.AddListener(() => OnSelectedItem(temp));
        }
        for (int i = 0; i < tabBtn.Length; i++)
        {
            int temp = i;
            tabBtn[i].onClick.AddListener(() => OnSelectedTab(temp));
        }
        for (int i = 0; i < equipBtn.Length; i++)
        {
            int temp = i;
            equipBtn[i].onClick.AddListener(() => OnSelectedEquip(temp));
        }
        for (int i = 0; i < useBtn.Length; i++)
        {
            int temp = i;
            useBtn[i].onClick.AddListener(() => OnSelectedUse(temp));
        }
    }

    void OnSelectedTab(int num) // 탭 버튼을 눌렀을 때 호출한다.
    {
        showEquipStat = false;
        selectedTab = num;
        selectedItem = 0;
        BtnChange(0);
        SelectedTab();
        ShowItem();
    }

    void OnSelectedItem(int num) // 아이템창의 아이템을 눌렀을 때 호출한다.
    {
        showEquipStat = false;
        selectedItem = num;
        canUse = true;
        BtnChange(1);
    }

    void OnSelectedEquip(int num) // 장비창을 눌렀을 때 호출한다.
    {
        showEquipStat = true; // 장비창을 눌렀는지 확인
        selectedEquip = num;
        BtnChange(2);
    }

    void ChangeTab()
    {
        if (selectedTab == 0)
        {
            use_Text.text = "사용하기";
            useBtnOnOff.SetActive(true);
            drop_Text.text = "버리기";
        }
        else if (selectedTab == 1)
        {
            use_Text.text = "착용하기";
            useBtnOnOff.SetActive(true);
            drop_Text.text = "버리기";
        }
        else if (selectedTab == 2)
        {
            use_Text.text = "버리기";
            drop_Text.text = " ";
            useBtnOnOff.SetActive(false);
        }
    }

    void BtnChange(int num) // 버튼을 눌렀을 때 변경되는 텍스트들과 버튼들을 관리한다.
    {
        switch (num)
        {
            case 0: // 탭을 선택했을 때 버튼
                if (!showEquipStat)
                {
                    icon.sprite = null;
                    description_Text.text = " ";
                    itemName_Text.text = " ";
                    ChangeTab();
                }
                break;
            case 1: // 아이템을 선택했을 때 버튼
                itemName_Text.text = inventoryTabList[selectedItem].itemName;
                description_Text.text = inventoryTabList[selectedItem].itemDescription;
                icon.sprite = inventoryTabList[selectedItem].itemIcon;
                ChangeTab();
                break;
            case 2: // 장비창을 선택했을 때 버튼
                canUse = true;
                use_Text.text = "착용해제";
                drop_Text.text = " ";
                itemName_Text.text = equipmentTakeOnList[selectedEquip].itemName;
                description_Text.text = equipmentTakeOnList[selectedEquip].itemDescription;
                icon.sprite = equipmentTakeOnList[selectedEquip].itemIcon;
                useBtnOnOff.SetActive(false);
                break;
            case 3: // 아이템을 다 사용했을 때 or 장비를 해제했을 때
                icon.sprite = null;
                description_Text.text = " ";
                itemName_Text.text = " ";
                canUse = false;
                break;
            default:
                break;
        }
    }

    void OnSelectedUse(int num) // 선택된 아이템을 사용하거나 버릴 때 호출한다.
    {
        if (canUse)
        {
            if (num == 0 && showEquipStat)
            {
                TakeOffEquip(selectedEquip);
            }
            else if (num == 0 && !showEquipStat)
            {
                UseItem();
            }
            if (num == 1)
            {
                RemoveItem();
            }
        }
    }
    private void EquipOnEffect(Item_Data _item)
    {
        //thePlayerStat.atk += _item.atk;
        //thePlayerStat.def += _item.def;
    }

    private void ConsumEffect(Item_Data _item)
    {
       // thePlayerStat.currentHp+= _item.plusHp;
        //thePlayerStat.currentMp += _item.plusMp;
    }

    private void EquipOffEffect(Item_Data _item)
    {
      //  thePlayerStat.atk -= _item.atk;
      //  thePlayerStat.def -= _item.def;
    }

    void TakeOffEquip(int num) // 착용한 장비를 해제한다.
    {
        inventoryItemList.Add(equipmentTakeOnList[num]);
        EquipOffEffect(equipmentTakeOnList[num]);
        equipmentTakeOnList.RemoveAt(num);
        BtnChange(3);
        ShowItem();
        ShowEquipment();
    }

    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    } // 탭 활성화

    public void GetAnItem(int _itemID, int _count=1)
    {
        for(int i=0; i<theDatabase.itemList.Count; i++) // 데이터베이스 아이템 검색
        {
            if (_itemID == theDatabase.itemList[i].itemID) // 아이템 찾음
            {
               for(int j=0; j<inventoryItemList.Count; j++) // 소지품에 같은 아이템이 있다 -> 갯수 증감 
                {
                    if (inventoryItemList[j].itemID == _itemID)
                    {
                        if (inventoryItemList[j].itemType != Item_Data.ItemType.Equipment)
                            inventoryItemList[j].itemCount += _count;
                        else
                            inventoryItemList.Add(theDatabase.itemList[i]);
                        return;
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]); // 없으면 소지품 해당 아이템 추가
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.Log("데이터베이스의 해당 id 값이 존재하지 않습니다 : Error");
    }
    public void RemoveSlot() // 아이템 슬롯을 초기화 시켜준다.
    {
        for(int i=0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    } 
    public void SelectedTab() // 선택한 탭의 색깔을 다르게 해준다.
    {
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        Color colora = color;
        colora.a = 0.5f;
        for(int i=0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        selectedTabImages[selectedTab].GetComponent<Image>().color = colora;
       // StartCoroutine(SelectedTabEffectCoroutine());
    }
   
    
    public void RemoveItem() // 버리기를 누르면 아이템 창에서 아이템을 지운다.
    {
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
            {   
                inventoryItemList.RemoveAt(i);
                ShowItem();
                break;
            }
        }
    }

    public void RemoveEquipSlot()
    {
        for (int i = 0; i < eslots.Length; i++)
        {
            eslots[i].RemoveItem();
            eslots[i].gameObject.SetActive(false);
        }
    }

    public void UseItem()
    {
        if (selectedTab == 0) // 소모품
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    if (inventoryItemList[i].itemCount > 1)
                        inventoryItemList[i].itemCount--;
                    else
                    {
                        inventoryItemList.RemoveAt(i);
                        BtnChange(3);
                    }
                    ConsumEffect(inventoryTabList[selectedItem]);
                    ShowItem();
                    break;
                }
            }
        }
        else if (selectedTab == 1)
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID && equipmentTakeOnList.Count < eslots.Length)
                {
                    equipmentTakeOnList.Add(inventoryTabList[selectedItem]);
                    inventoryItemList.RemoveAt(i);
                    BtnChange(3);
                    EquipOnEffect(inventoryTabList[selectedItem]);
                    ShowEquipment();
                    ShowItem();
                    break;
                }
            }
        }
    }

    public void ShowEquipment()
    {
        RemoveEquipSlot();
        for (int i = 0; i < equipmentTakeOnList.Count; i++)
        {
            eslots[i].gameObject.SetActive(true);
            eslots[i].AddItem(equipmentTakeOnList[i]);
        }
    }
    public void ShowItem() // 기존 아이템 슬롯을 초기화하고, 탭에 맞는 아이템들을 받아와서 보여준다.
    {
        int size;
        inventoryTabList.Clear(); //기존에 있던 슬롯들 초기화
        RemoveSlot();
        switch (selectedTab) //탭에 따른 분류, 그에 따른 아이템 리스트에 추가
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item_Data.ItemType.Consumer == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item_Data.ItemType.Equipment == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item_Data.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        }
        size = inventoryTabList.Count;
        if (selectedItem + 1 > size)  // 확인하기
            selectedItem -= 1;
        for (int i = 0; i < size; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // I키를 눌렀을 때 인벤토리 창을 열고 닫게 만든다.
        {
            activated = !activated;
            if (activated)
            {
                go.SetActive(true);
                selectedItem = 0;
                icon.sprite = null;
                description_Text.text = " ";
                itemName_Text.text = " ";
                ShowTab();
                ShowItem();
                ShowEquipment();
            }
            else
                go.SetActive(false);
        }
    }
}
