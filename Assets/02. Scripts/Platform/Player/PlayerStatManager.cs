using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatManager : Singleton<PlayerStatManager>
{
    [SerializeField] PlayerStatData playerStatData;

    private HPUI hpUI;
    private StatUI statUI;
    private PlayerController playerController;
    private bool invincibility = false;
    private GameObject player;
    private GameObject platformUI;
    private SpriteRenderer spr;
    private WaitForSeconds invincibilityTime = new WaitForSeconds(2f);
    public bool playerDead = false;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        platformUI = GameObject.FindGameObjectWithTag("PlatformUI");
        player = GameObject.FindGameObjectWithTag("Player");
        hpUI = platformUI.GetComponentInChildren<HPUI>();
        statUI = platformUI.GetComponentInChildren<StatUI>();
        playerController = player.GetComponent<PlayerController>();
        InitStat();
    }

    public void InitStat()
    {
        playerStatData.attackPower = 10;
        playerStatData.maxHP = 50;
        playerStatData.currentHP = playerStatData.maxHP;
        playerStatData.maxCost = 5;
        playerStatData.costRecoverySpeed = 3f;
        playerStatData.enhanceAtkStep = 0;
        playerStatData.enhanceHpStep = 0;
        ApplicationStat();
    }

    public void ApplicationStat()
    {
        hpUI.UpdateHPFigure();
        statUI.UpdateStatFigure();
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
                playerController.Dead();
                playerDead = true;
            }
            else
            {
                playerStatData.currentHP -= _damage;
                playerController.Hit();
            }
            hpUI.UpdateHPFigure();
        }
    }

    IEnumerator Invincibility()
    {
        invincibility = true;   
        yield return invincibilityTime;
        invincibility = false;
    }

    public void RespawnPlayer(bool _isFull, Transform _respawnPosition)
    {
        if (_isFull)
            playerStatData.currentHP = playerStatData.maxHP;
        else
            playerStatData.currentHP = playerStatData.maxHP / 2;
        playerController.Respawn(_respawnPosition);
        hpUI.UpdateHPFigure();
    }


}
