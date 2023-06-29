using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : BattleEntity
{
    private void Start()
    {
        hp = maxHp;
        Init();
        HpTextUpdate();
    }

    public override void Init()
    {
        battleBuffDebuff = gameObject.AddComponent<BattleBuffManager>();
    }

    public override void TimerControl(bool isStop)
    {
        throw new System.NotImplementedException();
    }

    public void MagicAttack(BattleEntity entity, float value)
    {
        entity.TakeDamage(value);
    }
}
