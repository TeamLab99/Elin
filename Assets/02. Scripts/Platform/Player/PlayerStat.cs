using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : MonoBehaviour
{
    public PlayerStatData playerStatData;
    int currentHP;
    int maxHP;
    int attackPower;
    int maxCost;
    float costRecoverySpeed;
    float currentMP;
    float maxMP;
    private void Awake()
    {
        currentHP = playerStatData.currentHP;
        currentMP = playerStatData.currentMP;
        maxHP = playerStatData.maxHP;
        maxMP = playerStatData.maxMP;
        attackPower = playerStatData.attackPower;
        maxCost = playerStatData.maxCost;
        costRecoverySpeed = playerStatData.costRecoverySpeed;
    }


}
