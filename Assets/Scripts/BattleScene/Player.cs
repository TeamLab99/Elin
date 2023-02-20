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
    }

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }
}