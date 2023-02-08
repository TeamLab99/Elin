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
    
    private bool itemActivated; //아이템 활성화 시 true
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

    void OnSelectedItem(int num)
    {
        showEquipStat = false;
        selectedItem = num;
        itemName_Text.text = inventoryTabList[selectedItem].itemName;
        description_Text.text = inventoryTabList[selectedItem].itemDescription;
        icon.sprite= inventoryTabList[selectedItem].itemIcon;
        SelectedTab();
        //UseItem();
    }

    void OnSelectedTab(int num)
    {
        showEquipStat = false;
        selectedTab = num;
        selectedItem = 0;
        Debug.Log(selectedTab);
        icon.sprite = null; 
        SelectedTab();
        ShowItem();
    }

    void OnSelectedEquip(int num)
    {
        showEquipStat = true;
        use_Text.text = "착용해제";
        drop_Text.text = " ";
        useBtnOnOff.SetActive(false);
        selectedEquip = num;
    }

    void OnSelectedUse(int num)
    {
        if (num == 0 && showEquipStat)
        {
            TakeOffEquip(selectedEquip);
        }
        else if (num == 0 && !showEquipStat)
        {
            UseItem();
        }
        if (num==1)
        {
            RemoveItem();
        }
    }

    void TakeOffEquip(int num) // 장비 착용 창 버튼
    {
        inventoryItemList.Add(equipmentTakeOnList[num]);
        equipmentTakeOnList.RemoveAt(num);
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
    public void RemoveSlot()
    {
        for(int i=0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    } // 인벤토리 슬롯 초기화
    public void SelectedTab()
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        Color colora = color;
        colora.a = 0.5f;
        for(int i=0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        selectedTabImages[selectedTab].GetComponent<Image>().color = colora;

     
        if (!showEquipStat)
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
        
       // StartCoroutine(SelectedTabEffectCoroutine());
    } // 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0
    
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (true)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color=color;
                yield return waitTime;            
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(5f);
        }
    } // 선택된 탭 반짝임 효과
    
    public void RemoveItem()
    {
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
            { inventoryItemList[i].itemCount=0;
                inventoryItemList.RemoveAt(i);
                ShowItem();
                break;
            }
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
                        inventoryItemList.RemoveAt(i);
                    ShowItem();
                    break;
                }
            }
        }
        else if (selectedTab == 1)
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    equipmentTakeOnList.Add(inventoryTabList[selectedItem]);
                    inventoryItemList.RemoveAt(i);
                    ShowEquipment();
                    ShowItem();
                    break;
                }
            }
        }
    }

    public void ShowEquipment()
    {
        int eslotSize=eslots.Length;
        int equipSize= equipmentTakeOnList.Count;
        int showSize=(eslotSize>equipSize)?equipSize:eslotSize;

        for (int i = 0; i < eslots.Length; i++)
        {
            eslots[i].RemoveItem();
            //eslots[i].gameObject.SetActive(false);
        }
        if(eslots.Length<selectedItem)
            selectedItem -= 1;
        for (int i = 0; i < showSize; i++)
        {
            eslots[i].AddItem(equipmentTakeOnList[i]);
            //eslots[i].gameObject.SetActive(true);
        }
    }
    public void ShowItem()
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
        if (selectedItem + 1 > size)
            selectedItem -= 1;
        for (int i = 0; i < size; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        }// 인벤토리 탭 리스트의 내용을 인벤토리 슬롯에 추가
        SelectedItem();
    } // 아이템 활성화(인벤토리탭리스트 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력
    public void SelectedItem()
    {
        StopAllCoroutines();
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            Color colora = color;
            colora.a = 0.5f;
            slots[selectedItem].selected_Item.GetComponent<Image>().color = colora;
            for (int i=0; i<inventoryTabList.Count; i++)
            {
                slots[i].selected_Item.GetComponent<Image>().color = color;
            }
            
            //StartCoroutine(SelectedItemEffectCoroutine());        
        }
        else
        {
            description_Text.text = " ";
            itemName_Text.text = " ";
        }
    } //선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값을 0으로 조정
    IEnumerator SelectedItemEffectCoroutine()
    {
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    } // 선택된 아이템 반짝임 효과
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activated = !activated;
            if (activated)
            {
                go.SetActive(true);
                //itemActivated = false;
                ShowTab();
                ShowItem();
            }
            else
            {
                go.SetActive(false);
                StopAllCoroutines();
                //itemActivated = false;
            }
        }
    }
}
