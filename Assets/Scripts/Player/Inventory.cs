using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    
    private Inventory_Slot[] slots; // 인벤토리 슬롯들
    private List<Item_Data> inventoryItemList; //플레이어가 소지한 아이템 리스트
    private List<Item_Data> inventoryTabList; // 선택한 템에 따라 다르게 보여질 아이템 리스트
    public Text description_Text; // 부연설명
    public string[] tabDescription; // 탭 부연설명
    public Transform tf; // 슬롯의 부모객체

    public GameObject go; //인벤토리 활성화 비활성하
    public GameObject[] selectedTabImages;
    //public GameObject selectionWindow;
    private int selectedItem; //선택된 아이템
    private int selectedTab; //선택된 탭
    private bool activated; //인벤토리 활성화시 true
    private bool tabActivated; //탭 활성화 시 true
    private bool itemActivated; //아이템 활성화 시 true
    private bool stopKeyInput; // 키 입력 제한 (소비할 때 나오는 질의 중에)
    private bool preventExec; //중복 실행 제한

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    void Start()
    {
        inventoryItemList = new List<Item_Data>();
        inventoryTabList = new List<Item_Data>();
        slots = tf.GetComponentsInChildren<Inventory_Slot>();
        inventoryItemList.Add(new Item_Data(10001, "cherry", "체력을 10 증가", Item_Data.ItemType.Consumer));
        inventoryItemList.Add(new Item_Data(10002, "gem", "체력을 30 증가", Item_Data.ItemType.Consumer));
        inventoryItemList.Add(new Item_Data(20001, "bag", "방어력 10 증가", Item_Data.ItemType.Equipment));
        inventoryItemList.Add(new Item_Data(20002, "belt", "방어력 30 증가", Item_Data.ItemType.Equipment));
        inventoryItemList.Add(new Item_Data(30001, "bone", "해골몬스터의 뼈조각", Item_Data.ItemType.Quest));
        inventoryItemList.Add(new Item_Data(30002, "egg", "닭 몬스터의 알", Item_Data.ItemType.Quest));
        inventoryItemList.Add(new Item_Data(40001, "silk", "방어구의 재료", Item_Data.ItemType.Etc));
        inventoryItemList.Add(new Item_Data(40010, "diamond", "무기의 재료", Item_Data.ItemType.Etc));
    }

   public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    } // 탭 활성화
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
        for(int i=0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    } // 선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tabActivated)
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
            yield return new WaitForSeconds(0.3f);
        }
    } // 선택된 탭 반짝임 효과
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
            for(int i=0; i<inventoryTabList.Count; i++)
            {
                slots[i].selected_Item.GetComponent<Image>().color = color;
            }
            description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());        
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
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;
                if (activated)
                {
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else
                {
                    go.SetActive(false);
                    StopAllCoroutines();
                    tabActivated = false;
                    itemActivated = false;
                }
            }
            if (activated)
            {
                if (tabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab >0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length-1;
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();
                    }
                } // 템 활성화 시 키 입력 처리
                else if (itemActivated) // 아이템 활성화시 키입력 처리
                {
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
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0) // 소모품
                            {
                                stopKeyInput = true;
                                // 물약을 마실거냐 혹은 같은 선택지 호출

                            }
                            else if (selectedTab == 1)
                            {
                                // 장비 장착
                            }
                            else
                            {
                                //비프음 출력
                            }
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Z)) /// 중복처리 방지
                    preventExec = false;
            }
        }
    }
}
