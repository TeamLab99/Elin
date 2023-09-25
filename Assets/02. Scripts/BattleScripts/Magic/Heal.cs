using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : BuffDebuffMagic
{

    private void Awake()
    {
        Managers.Data.CardDict.TryGetValue(3, out card);

        probability = card.attackProbability;
        mainTime = card.buffMaintainTime;
        amount = card.amount;
    }

    public override IEnumerator Timer()
    {
        yield return null;
    }
}
