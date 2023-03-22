using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Player : Entity
{
    protected Monster monster;
    [SerializeField] int cost;
    [SerializeField] int costRenewable;
    [SerializeField] GameObject hitEffect;

    protected override void Start()
    {
        base.Start();
        HPTxtUpdate();
        monster = GameObject.FindGameObjectWithTag("Mob").GetComponent<Monster>();
    }

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }

    public void HitEffectOn()
    {
        hitEffect.SetActive(true);
    }

    public void HitEffectOff()
    {
        hitEffect.SetActive(false);
    }
}