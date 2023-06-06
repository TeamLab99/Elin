using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : MonoBehaviour
{
    [SerializeField] PlayerStatData playerStatData;
    
    HPUI hpUI;
    StatUI statUI;

    private void Awake()
    {
        hpUI = FindObjectOfType<HPUI>();
        statUI = FindObjectOfType<StatUI>();
    }

    public void ChangeStat(int _maxHP=0, int _attackPower=0, int _maxCost=0, float _recoverySpeed=0)
    {
        playerStatData.maxHP += _maxHP;
        playerStatData.attackPower += _attackPower;
        playerStatData.maxCost += _maxCost;
        playerStatData.costRecoverySpeed += _recoverySpeed;
        statUI.UpdateStatFigure();
    }

    public void HealPlayer(int _heal)
    {
        if (playerStatData.currentHP + _heal > playerStatData.maxHP)
            playerStatData.currentHP = playerStatData.maxHP;
        else
            playerStatData.currentHP += _heal;
        hpUI.UpdateHPFigure();
    }

    public void DamagePlayer(int _damage)
    {
        if (playerStatData.currentHP - _damage <= 0)
        {
            playerStatData.currentHP = 0;
            Debug.Log("플레이어는 죽었습니다..");
        }
        else
            playerStatData.currentHP -= _damage;
        hpUI.UpdateHPFigure();
    }
}
