using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] Image itemIcon;
    [SerializeField] Text itemCntText;
    private Button button;
    private Items holdItemInfo;
    Color transparentColor = Color.white;
    Color nontransparentColor = Color.white;
    private int itemIdx;

    [SerializeField] private Image dragImage;
    [SerializeField] private RectTransform dragRectTransform;
    private Vector2 originalPosition;
    private bool isDragging;
    private Sprite originalImage;

    GameObject droppedButton;
    Image droppedImage;
    ShortCutKeySlot shortCutKeySlot;


    private void Awake()
    {
        transparentColor.a = 0f;
        nontransparentColor.a = 1f;
        isDragging = false;
    }

    public void AddItem(Items _itemData, int _itemIdx)
    {
        itemIcon.color = nontransparentColor;
        itemIcon.sprite = _itemData.itemIcon;
        holdItemInfo = _itemData;
        itemIdx = _itemIdx;        
        if (_itemData.itemType != "Equ")
            itemCntText.text = "X " + _itemData.itemCnt.ToString();
        else
            itemCntText.text = " ";
    }

    public void RemoveItem()
    {
        itemCntText.text = " ";
        itemIcon.sprite = null;
        transparentColor.a = 0f;
        itemIcon.color = transparentColor;
    } 

    public void ClickItemSlot()
    {
        if (itemIcon.sprite == null)
        {
            InvenUI.instance.itemInfoObject.SetActive(false);
            return;
        }
        InvenUI.instance.itemInfoObject.SetActive(true);
        InvenUI.instance.itemInfoUI.ChangeItemInfo(holdItemInfo, itemIdx);
    }

    public void OnPointerClick(PointerEventData eventData) // 우클릭
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ClickItemSlot();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            originalPosition = dragRectTransform.anchoredPosition;
            isDragging = true;
            originalImage = dragImage.sprite;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 position = eventData.position;
            dragRectTransform.position = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            if(eventData.pointerCurrentRaycast.gameObject.layer == 22)
            {
                droppedButton = eventData.pointerCurrentRaycast.gameObject;
                shortCutKeySlot = droppedButton.GetComponent<ShortCutKeySlot>();
            }
            if (droppedButton != null && droppedButton != gameObject)
            {

                shortCutKeySlot.ChangeItemInfo(holdItemInfo, itemIdx);
            }
            dragRectTransform.anchoredPosition = originalPosition;
            isDragging = false;
        }
    }
}
