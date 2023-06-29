using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadButt : AttackMagic
{
    private void Awake()
    {
        Managers.Data.CardDict.TryGetValue(2, out card);

        probability = card.attackProbability;
        percent = card.attackPercent;
        amount = card.amount;
    }

    public override float CalculateAttackValue(float value)
    {
        return amount;
    }
}
