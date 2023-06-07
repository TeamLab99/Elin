using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatUI : MonoBehaviour
{
    [SerializeField] PlayerStatData playerStatData;
    [SerializeField] Text maxHPText;
    [SerializeField] Text attackPowerText;
    [SerializeField] Text maxCostText;
    [SerializeField] Text costRecoverySpeedText;


    private void Start()
    {
        UpdateStatFigure();
    }

    public void UpdateStatFigure()
    {
        maxHPText.text = "+"+playerStatData.maxHP.ToString();
        attackPowerText.text = "+" + playerStatData.attackPower.ToString();
        maxCostText.text = "+" + playerStatData.maxCost.ToString();
        costRecoverySpeedText.text = "+" + playerStatData.costRecoverySpeed.ToString();
    }
}
