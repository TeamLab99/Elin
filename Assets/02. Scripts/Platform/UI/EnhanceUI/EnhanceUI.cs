using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceUI : Singleton<EnhanceUI>
{
    [SerializeField] PlayerStatData playerStatData;
    public EnhanceStat enhanceStat;
    public EnhanceBtn[] enhanceBtns;
    public GameObject enhanceDecisionUI;
    public Text[] enhanceDecisionStatText;
    public string enhanceDecisionStatString;
    public GameObject enhanceCompleteUI;
    public int[] enhanceStatFigure;
    public int[] enhancePrice;
    public int selectStatEnhancePrice;
    public int enhanceStep;
    public Text aumText;

    private int btnIndex;
    private bool canUpgrade=true;


    public void SelectStat(EnhanceStat _enhanceStat)
    {
        enhanceStat = _enhanceStat;
        SetSelectStatEnhancePrice();
        for (int i=0; i<enhanceBtns.Length; i++)
        {
            if (enhanceBtns[i].enhanceStat == enhanceStat)
            {
                enhanceBtns[i].selectIcon.SetActive(true);
                btnIndex = i;
            }
            else
                enhanceBtns[i].selectIcon.SetActive(false);
        }
    }

    public void SetSelectStatEnhancePrice()
    {
        switch (enhanceStat)
        {
            case EnhanceStat.Health:
                enhanceStep = playerStatData.enhanceHpStep;
                enhanceDecisionStatString = "최대 체력";
                break;
            case EnhanceStat.Attack:
                enhanceStep = playerStatData.enhanceAtkStep;
                enhanceDecisionStatString = "공격력";
                break;
            case EnhanceStat.MaxCost:
                enhanceStep = playerStatData.enhanceCostStep;
                enhanceDecisionStatString = "최대 코스트";
                break;
            case EnhanceStat.CostRecovery:
                enhanceStep = playerStatData.enhanceRecoveryStep;
                enhanceDecisionStatString = "코스트 회복력";
                break;
        }
        if (enhanceStep == 3)
        {
            canUpgrade = false;
            return;
        }
        selectStatEnhancePrice = enhancePrice[enhanceStep];
        canUpgrade = true;
    }
    public void SelectUpgradeBtn()
    {
        if (canUpgrade)
        {
            enhanceDecisionUI.SetActive(true);
            enhanceDecisionStatText[0].text = enhanceDecisionStatString;
            enhanceDecisionStatText[1].text = (enhanceStep + 1).ToString() + "단계";
            enhanceDecisionStatText[2].text = (enhanceStep).ToString() + "단계";
            enhanceDecisionStatText[3].text = selectStatEnhancePrice.ToString();
        }    
    }

    public void ExitEnhanceUI()
    {
        enhanceStat = EnhanceStat.None;
        gameObject.SetActive(false);
        enhanceDecisionUI.SetActive(false);
        enhanceCompleteUI.SetActive(false);
    }

    public void SelectDecisionEnhanceUI()
    {
        if(ItemManager.instance.LoadAum()>= selectStatEnhancePrice)
        {
            ItemManager.instance.UseAum(selectStatEnhancePrice);
            switch (enhanceStat)
            {
                case EnhanceStat.Health:
                    playerStatData.enhanceHpStep += 1;
                    playerStatData.maxHP += enhanceStatFigure[(int)EnhanceStat.Health];
                    break;
                case EnhanceStat.Attack:
                    playerStatData.enhanceAtkStep += 1;
                    playerStatData.attackPower += enhanceStatFigure[(int)EnhanceStat.Attack];
                    break;
                case EnhanceStat.MaxCost:
                    playerStatData.enhanceCostStep += 1;
                    playerStatData.maxCost += enhanceStatFigure[(int)EnhanceStat.MaxCost];
                    break;
                case EnhanceStat.CostRecovery:
                    playerStatData.enhanceRecoveryStep += 1;
                    playerStatData.costRecoverySpeed += enhanceStatFigure[(int)EnhanceStat.CostRecovery];
                    break;
            }
            enhanceBtns[btnIndex].SetUpFigure();
            enhanceDecisionUI.SetActive(false);
            enhanceCompleteUI.SetActive(true);
            aumText.text = ItemManager.instance.LoadAum().ToString();
        }
        else
        {
            return;
        }
    }

    public void ExitDecisionEnhanceUI()
    {
        enhanceDecisionUI.SetActive(false);
    }

    public void CheckCompleteEnhanceUI()
    {
        enhanceCompleteUI.SetActive(false);
    }

    public void OnEnable()
    {
        aumText.text = ItemManager.instance.LoadAum().ToString(); 
    }
}
