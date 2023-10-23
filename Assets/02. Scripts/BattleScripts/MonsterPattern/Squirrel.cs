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
                count = skillCount;
                curTime = maxTime;
            }
            else
            {
                count = skillCount;
                curTime = maxTime;
                StartCoroutine(Attack());
            }
        }
        yield return null;
    }

    public IEnumerator Attack()
    {
        ChangeAnim(EMonsterState.Attack);
        EntitiesStateChange(true);
        //gameObject.transform.DOScale(Vector3.one, 0.5f).SetRelative().SetEase(Ease.Flash, 2, 0);

        yield return StartCoroutine(MobSkillManager.instance.CallNormalAttackEffect(1));
        Attack(player);
        EntitiesStateChange(false);
        AnimationControl();
        ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator Skill()
    {
        ChangeAnim(EMonsterState.Skill);
        EntitiesStateChange(true);

        attackSpeed -= 0.5f;
        maxTime = attackSpeed;
        skillOverlap++;

        yield return StartCoroutine(MobSkillManager.instance.Broadening());
        EntitiesStateChange(false);
        ChangeAnim(EMonsterState.Idle);
    }

    public void AnimationControl()
    {
        iconAnimation.Animation(1);
    }

    public override void TakeDamage(float value)
    {
        if (hp - value > 0)
        {
            hp -= value;
        }
        else
        {
            hp = 0;
            ChangeAnim(EMonsterState.Death);
            StartCoroutine(BattleGameManager.instance.isDeadMotionEnd());
        }
        HpTextUpdate();
    }

    public void Revolution()
    {
        BattleMagicManager.instance.ClearBuff();
        EntitiesStateChange(true);
        TimerControl(true);
        iconAnimation.TimerControl(true);
        StopAllCoroutines();
        FadeOut();
    }

    public void FadeOut()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GetComponent<SpriteRenderer>().DOFade(0, 1f))
        .AppendInterval(0.5f)
            .OnComplete(() =>
            {
                BattleGameManager.instance.GenerateNightmare();
            });
    }
}
