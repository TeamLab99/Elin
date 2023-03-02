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

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        HPTxtUpdate();
        var mob = GameObject.FindGameObjectWithTag("Mob");

        monster = mob.GetComponent<Monster>();
    }

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }
}