using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenTabSlot : MonoBehaviour
{
    public ItemTypes thisTabType;
    private Image buttonImage;
    private InvenUI invenUI;
    private Button button;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        invenUI = GetComponentInParent<InvenUI>();
        button.onClick.AddListener(() =>invenUI.ChangeTab(thisTabType));
        SelectButtonEffect();
    }

    public void SelectButtonEffect()
    {
        if(invenUI.itemTypeEnum == thisTabType)
            buttonImage.color = Color.red;
        else
            buttonImage.color = Color.white;
    }


}
