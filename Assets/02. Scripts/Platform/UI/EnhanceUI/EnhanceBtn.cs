using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnhanceBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EnhanceStat enhanceStat;
    public GameObject selectIcon;

    private Image image;
    private Color originalColor;
    private Color transparentColor;
    [SerializeField] Text statName;
    [SerializeField] Text stepFigureText;
    [SerializeField] Text priceFigureText;
    [SerializeField] PlayerStatData playerStatData;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        SetName();
        SetUpFigure();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = transparentColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = originalColor;
    }

    public void ClickBtn()
    {
        EnhanceUI.instance.SelectStat(enhanceStat);
    }

    public void SetUpFigure()
    {
        switch (enhanceStat)
        {
            case EnhanceStat.Health:
                stepFigureText.text = playerStatData.enhanceHpStep.ToString();
                if (playerStatData.enhanceHpStep == 3)
                    priceFigureText.text = "강화 최대";
                else
                    priceFigureText.text=EnhanceUI.instance.enhancePrice[playerStatData.enhanceHpStep].ToString();
                break;
            case EnhanceStat.Attack:
                stepFigureText.text = playerStatData.enhanceAtkStep.ToString();
                if (playerStatData.enhanceAtkStep == 3)
                    priceFigureText.text = "강화 최대";
                else
                    priceFigureText.text = EnhanceUI.instance.enhancePrice[playerStatData.enhanceAtkStep].ToString();
                break;
            case EnhanceStat.MaxCost:
                stepFigureText.text = playerStatData.enhanceCostStep.ToString();
                if (playerStatData.enhanceCostStep == 3)
                    priceFigureText.text = "강화 최대";
                else
                    priceFigureText.text = EnhanceUI.instance.enhancePrice[playerStatData.enhanceCostStep].ToString();
                break;
            case EnhanceStat.CostRecovery:
                stepFigureText.text = playerStatData.enhanceRecoveryStep.ToString();
                if (playerStatData.enhanceRecoveryStep == 3)
                    priceFigureText.text = "강화 최대";
                else
                    priceFigureText.text = EnhanceUI.instance.enhancePrice[playerStatData.enhanceRecoveryStep].ToString();
                break;
        }
    }

    public void SetName()
    {
        switch (enhanceStat)
        {
            case EnhanceStat.Health:
                statName.text = "최대 체력";
                break;
            case EnhanceStat.Attack:
                statName.text = "공격력";
                break;
            case EnhanceStat.MaxCost:
                statName.text = "최대 코스트";
                break;
            case EnhanceStat.CostRecovery:
                statName.text = "코스트 회복력";
                break;
        }
    }
}
