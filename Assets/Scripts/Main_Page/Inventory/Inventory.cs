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
    public string[] tabDescription; // 탭 부연설명
    public Transform tf; // 슬롯의 부모객체
    public Transform etf;
    private RectTransform rectTransform;
    public Button[] btn;

    public GameObject go; //인벤토리 활성화 비활성하
    public GameObject[] selectedTabImages;
    
    //public GameObject selectionWindow;
    private int selectedItem; //선택된 아이템
    private int selectedTab; //선택된 탭
    private bool activated; //인벤토리 활성화시 true
    
    private bool itemActivated; //아이템 활성화 시 true
    private bool stopKeyInput; // 키 입력 제한 (소비할 때 나오는 질의 중에)

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    private Equipment theEquip;


    void Start()
    {
        instance = this;
        theDatabase = FindObjectOfType<DataBase_Manager>(); // 한번 접근이 아니라 여러번 접근이므로 find사용
        inventoryItemList = new List<Item_Data>();
        inventoryTabList = new List<Item_Data>();
        equipmentTakeOnList = new List<Item_Data>();
        slots = tf.GetComponentsInChildren<Inventory_Slot>();
        eslots = etf.GetComponentsInChildren<Equipment_Slot>();
        rectTransform = GetComponent<RectTransform>();

        for(int i=0; i<btn.Length; i++)
        {
            int temp = i;
            btn[i].onClick.AddListener(() => OnSelectedItem(temp));
        }
    }
    void OnSelectedItem(int num)
    {
        selectedItem = num;
        Debug.Log(selectedItem);
        UseItem();
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
                        if (inventoryItemList[i].itemType != Item_Data.ItemType.Equipment)
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
        description_Text.text = tabDescription[selectedTab];
        selectedTabImages[selectedTab].GetComponent<Image>().color = colora;
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
        //int a, b;
        
        for (int i = 0; i < eslots.Length; i++)
        {
            eslots[i].RemoveItem();
            //eslots[i].gameObject.SetActive(false);
        }
        selectedItem = 0;
        for (int i = 0; i < eslots.Length; i++)
        {
            eslots[i].AddItem(equipmentTakeOnList[i]);
            //eslots[i].gameObject.SetActive(true);
        }
    }
    public void ShowItem()
    {
        inventoryTabList.Clear(); //기존에 있던 슬롯들 초기화
        RemoveSlot();
        selectedItem = 0;
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
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item_Data.ItemType.Etc == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        }
        for (int i = 0; i < inventoryTabList.Count; i++)
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
            description_Text.text = inventoryTabList[selectedItem].itemDescription;
            
            //StartCoroutine(SelectedItemEffectCoroutine());        
        }
        else
        {
            description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
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
        if (activated)
        {
            itemActivated = true;
            

            if (inventoryTabList.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedItem < inventoryTabList.Count - 2)
                        selectedItem += 2;
                    else
                        selectedItem %= 2;
                    SelectedItem();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedItem > 1)
                        selectedItem -= 2;
                    else
                        selectedItem = inventoryTabList.Count - 1 - selectedItem;
                    SelectedItem();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedItem < inventoryTabList.Count - 1)
                        selectedItem++;
                    else
                        selectedItem = 0;
                    SelectedItem();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedItem > 0)
                        selectedItem--;
                    else
                        selectedItem = inventoryTabList.Count - 1;
                    SelectedItem();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (selectedTab == 0) // 소모품
                    {
                        //stopKeyInput = true;  
                        //아이템 소모 시 yes or no 선택창을 안쓰므로 필요없음
                        // 물약을 마실거냐 혹은 같은 선택지 호출
                        UseItem();
                    }
                    else if (selectedTab == 1)
                    {
                        // 장비 장착
                        theEquip.EquipItem(inventoryItemList[selectedItem]);
                        inventoryItemList.RemoveAt(selectedItem);
                        ShowItem();
                    }
                    else
                    {
                        //비프음 출력
                    }
                }
            }
        }
    }
    public void ConsumerTab()
    {
        selectedTab = 0;
        SelectedTab();
        ShowItem();
    }
    public void EquipTab()
    {
        selectedTab = 1;
        SelectedTab();
        ShowItem();
    }
    public void QuestTab()
    {
        selectedTab = 2;
        SelectedTab();
        ShowItem();
    }
    public void EctTab()
    {
        selectedTab = 3;
        SelectedTab();
        ShowItem();
    }
}
