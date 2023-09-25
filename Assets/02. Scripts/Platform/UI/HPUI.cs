using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPUI : MonoBehaviour
{
    [SerializeField] PlayerStatData playerStatData;
    
    public Slider slider;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        UpdateHPFigure();
    }


    public void UpdateHPFigure()
    {
        slider.maxValue = playerStatData.maxHP;
        slider.value = playerStatData.currentHP;
    }

}
