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
        BattleGameManager.instance.ChangeAnim(EMonsterState.Attack);
        EntitiesStateChange(true);
        //gameObject.transform.DOScale(Vector3.one, 0.5f).SetRelative().SetEase(Ease.Flash, 2, 0);

        yield return StartCoroutine(MobSkillManager.instance.CallNormalAttackEffect(1));
        Attack(player);
        EntitiesStateChange(false);
        AnimationControl();
        BattleGameManager.instance.ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator Skill()
    {
        BattleGameManager.instance.ChangeAnim(EMonsterState.Skill);
        EntitiesStateChange(true);

        attackSpeed -= 0.5f;
        maxTime = attackSpeed;
        skillOverlap++;

        yield return StartCoroutine(MobSkillManager.instance.Broadening());
        EntitiesStateChange(false);
        BattleGameManager.instance.ChangeAnim(EMonsterState.Idle);
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
            BattleGameManager.instance.ChangeAnim(EMonsterState.Death);
            StartCoroutine(BattleGameManager.instance.isDeadMotionEnd());
        }
        HpTextUpdate();
    }

    public void Revolution()
    {
        EntitiesStateChange(true);
        TimerControl(true);
        iconAnimation.TImerControl(false);
        StopCoroutine(GaugeTimer());
        StartCoroutine(MobSkillManager.instance.Revolution());
    }

    public void FadeOut()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GetComponent<SpriteRenderer>().DOFade(0, 1f))
            .OnComplete(() =>
            {
                BattleGameManager.instance.GenerateNightmare();
            });
        
    }
}
