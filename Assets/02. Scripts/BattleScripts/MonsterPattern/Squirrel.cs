/*using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Squirrel : Monster
{
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator MonsterPattern()
    {
        if (count > 0)
        {
            curTime = maxTime;

            StartCoroutine(AttackOfMonster());
            count--;
        }
        else if (count == 0)
        {
            if (maxTime > 1f)
            {
                UseSkill(skillindex);
                maxTime = attackSpeed;
            }
            count = skillCount;
        }

        yield return null;
    }
}
*/