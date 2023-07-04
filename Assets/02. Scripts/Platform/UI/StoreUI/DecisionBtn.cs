using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionBtn : MonoBehaviour
{
    [SerializeField] bool isBuy;
    [SerializeField] GameObject warningText;
    
    public void ClickBtn()
    {
        if (isBuy)
        {
            if (StoreUI.instance.CanBuyObject())
            {
                StoreUI.instance.OffDecisionBuy();
            }   
            else
            {
                warningText.SetActive(true);
            }
        }
        else
        {
            warningText.SetActive(false);
            StoreUI.instance.OffDecisionBuy();
        }
    }
}
