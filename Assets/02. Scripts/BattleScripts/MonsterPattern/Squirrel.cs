using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Squirrel : BattleMonster
{
    int skillOverlap = 0;

    protected override IEnumerator MonsterPattern(int skillCount)
    {
        if (count > 0)
        {
            curTime = maxTime;
            StartCoroutine(Attack());
            count--;
        }
        else
        {
            if (skillOverlap < 2)
            {
                Debug.Log("스킬 발동");
                StartCoroutine(Skill());
            }

            count = skillCount;
        }
        yield return null;
    }

    public IEnumerator Attack()
    {
        EntitiesStateChange(true);
        //gameObject.transform.DOScale(Vector3.one, 0.5f).SetRelative().SetEase(Ease.Flash, 2, 0);

        yield return StartCoroutine(MobSkillManager.instance.CallNormalAttackEffect(1));
        Attack(player);
        EntitiesStateChange(false);
        AnimationControl();
    }

    public IEnumerator Skill()
    {
        EntitiesStateChange(true);

        attackSpeed -= 0.5f;
        maxTime = attackSpeed;
        skillOverlap++;

        yield return StartCoroutine(MobSkillManager.instance.Broadening());
        EntitiesStateChange(false);
        AnimationControl();
    }

    public void AnimationControl()
    {
        iconAnimation.Animation(maxTime);
    }
}
