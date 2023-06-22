using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatStepUI : MonoBehaviour
{
    public Text enhanceHpStepText;
    public Text enhanceAtkStepText;
    public Text enhanceCostStepText;
    public Text enhanceRecoveryStepText;

    public Text enhanceHpStepFigureText;
    public Text enhanceAtkStepFigureText;
    public Text enhanceCostStepFigureText;
    public Text enhanceRecoveryStepFigureText;

    [SerializeField] PlayerStatData playerStatData;

    private void Start()
    {
        UpdateEnhanceStep();
        UpdateStatFigure();
    }

    public void UpdateEnhanceStep()
    {
        enhanceHpStepText.text = "+ " + playerStatData.enhanceHpStep.ToString();
        enhanceAtkStepText.text = "+ " + playerStatData.enhanceAtkStep.ToString();
        enhanceCostStepText.text = "+ " + playerStatData.enhanceCostStep.ToString();
        enhanceRecoveryStepText.text = "+ " + playerStatData.enhanceRecoveryStep.ToString();
    }

    public void UpdateStatFigure()
    {
        enhanceHpStepFigureText.text = "(+ " + playerStatData.enhanceHpStep.ToString() +")";
        enhanceAtkStepFigureText.text = "(+ " + playerStatData.enhanceAtkStep.ToString() + ")";
        enhanceCostStepFigureText.text = "(+ " + playerStatData.enhanceCostStep.ToString() + ")";
        enhanceRecoveryStepFigureText.text = "(+ " + playerStatData.enhanceRecoveryStep.ToString() + ")";
    }
}
