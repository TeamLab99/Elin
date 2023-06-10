using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : Singleton<PlayerStat>
{
    [SerializeField] PlayerStatData playerStatData;

    private HPUI hpUI;
    private StatUI statUI;
    private PlayerMove playerMove;
    private bool invincibility = false;
    private GameObject player;
    private GameObject platformUI;
    private WaitForSeconds invincibilityTime = new WaitForSeconds(2f);

    private void Awake()
    {
        platformUI = GameObject.FindGameObjectWithTag("PlatformUI");
        player = GameObject.FindGameObjectWithTag("Player");
        hpUI = platformUI.GetComponentInChildren<HPUI>();
        statUI = platformUI.GetComponentInChildren<StatUI>();
        playerMove = player.GetComponent<PlayerMove>();
    }

    public void ChangeStat(int _maxHP=0, int _attackPower=0, int _maxCost=0, float _recoverySpeed=0)
    {
        playerStatData.maxHP += _maxHP;
        playerStatData.attackPower += _attackPower;
        playerStatData.maxCost += _maxCost;
        playerStatData.costRecoverySpeed += _recoverySpeed;
        statUI.UpdateStatFigure();
        hpUI.UpdateHPFigure();
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
        if (!invincibility)
        {
            StartCoroutine("Invincibility");
            if (playerStatData.currentHP - _damage <= 0)
            {
                playerStatData.currentHP = 0;
                playerMove.Dead();
            }
            else
                playerStatData.currentHP -= _damage;
            hpUI.UpdateHPFigure();
        }
    }

    IEnumerator Invincibility()
    {
        invincibility = true;
        yield return invincibilityTime;
        invincibility = false;
    }
}
