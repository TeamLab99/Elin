using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : Monster
{
    int random;
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator MonsterPattern()
    {
        if (count > 0)
        {
            curTime = maxTime;
            random = Random.Range(0, 10);
            if (random == 9)
                attack *= 1.5f;
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
