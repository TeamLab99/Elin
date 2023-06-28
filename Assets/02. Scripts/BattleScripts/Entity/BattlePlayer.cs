using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : BattleEntity
{
    public override void BuffCheck()
    {
        throw new System.NotImplementedException();
    }

    public override void Init()
    {
        battleBuffDebuff = gameObject.AddComponent<BattleBuffManager>();
    }
}
