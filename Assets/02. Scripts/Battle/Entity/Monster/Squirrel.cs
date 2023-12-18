using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Squirrel : Monster
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
            StartCoroutine(Dead());
        }

        HpTextUpdate();
    }

    public void Run()
    {
        ChangeAnim(EMonsterState.Run);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(gameObject.transform.DOMoveX(transform.position.x+10f, 2f))
            .AppendInterval(3f)
            .OnComplete(() =>
            {
                StopAllCoroutines();
                Destroy(gameObject);
            });
    }
    
    public IEnumerator Dead()
    {
        MagicManager.instance.ClearBuff();
        TimerControl(true);
        EntitiesStateChange(true);
        
        yield return new WaitForSeconds(1f);
        TimerControl(true);
        EntitiesStateChange(true);
        BattleManager.instance.BattleEnd();
        
        iconAnimation.Reset();
        gauge.fillAmount = 1f;
        Run();
        
        CardManager.instance.DisableCards();
        
        yield return new WaitForSeconds(3f);
        BattleManager.instance.ResetCamera();
        //DialogueManager.instance.SetEvent();
        DialogueManager.instance.NextDialogue("Erica");
        DialogueManager.instance.StartDialogue("Erica"); //대화 불러오기 Erica8 불러옴
        Debug.Log("111");   
    }
}