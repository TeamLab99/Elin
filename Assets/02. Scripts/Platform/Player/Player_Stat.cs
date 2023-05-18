using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stat : MonoBehaviour
{
    [SerializeField]private int maxHP;
    private int currentHP;
    private int attackPower;
    private int maxCost;
    private float recoveryCost;
    [SerializeField] Slider hpSlider;
    Player_Move playerMove;

    void Awake()
    {
        LoadStat(PlatForm_Manager.instance.currentHP, PlatForm_Manager.instance.attackPower, PlatForm_Manager.instance.maxCost, PlatForm_Manager.instance.recoveryCost);
        playerMove = GetComponent<Player_Move>();
        ApplicationHP();
    }

    public void HealHP(int heal)
    {
        if (maxHP < heal + currentHP)
            currentHP = maxHP;
        else
            currentHP += heal;
        ApplicationHP();
    }

    public void DamageHP(int damage)
    {
        if (currentHP - damage <= 0)
            playerMove.Dead();
        else
            currentHP -= damage;
        ApplicationHP();
    }

    public void StatUP(int plusHP = 0, int plusAttack=0, int plusCost=0, float plusRecoveryCost=0)
    {
        maxHP += plusHP;
        currentHP += plusHP;
        attackPower += plusAttack;
        maxCost += plusCost;
        recoveryCost += plusRecoveryCost;
        ApplicationHP();
    }

    void SaveStat()
    {
        PlatForm_Manager.instance.currentHP = currentHP;
        PlatForm_Manager.instance.attackPower = attackPower;
        PlatForm_Manager.instance.maxCost = maxCost;
        PlatForm_Manager.instance.recoveryCost = recoveryCost;
    }

    void LoadStat(int hp, int atk, int cost, float recovery)
    {
        currentHP = hp;
        attackPower = atk;
        maxCost = cost;
        recoveryCost = recovery;
    }

    void ApplicationHP()
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }
}
