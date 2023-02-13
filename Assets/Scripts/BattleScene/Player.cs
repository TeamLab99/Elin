using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Player : Entity
{
    [SerializeField] int cost;
    [SerializeField] int costRenewable;

    void Start()
    {
        base.HealthUpdate();
    }
}
