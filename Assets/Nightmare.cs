using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Nightmare : BattleMonster
{
    int brodeningOverlapValue = 0;

    public override void Init()
    {
        ConnectInspector();

        delay = new WaitForSeconds(Time.deltaTime);
        maxTime = attackSpeed;
        curTime = maxTime;
        count = skillCount;

        FadeIn();
    }

    protected override IEnumerator MonsterPattern(int skillCount)
    {
        if (count > 0)
        {
            count--;
            curTime = maxTime;
            StartCoroutine(Attack());
        }
        else
        {
            StartCoroutine(Skill());
            count = skillCount;
            curTime = maxTime;
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

        var choice = Random.Range(0, 3);

        choice = 2;
        switch (choice)
        {
            case 1:
                yield return StartCoroutine(Broadening());
                break;
            case 2:
                yield return StartCoroutine(MobSkillManager.instance.Fear());
                break;
            default:
                break;
        }

        EntitiesStateChange(false);
        BattleGameManager.instance.ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator Broadening()
    {
        if (brodeningOverlapValue < 4)
        {
            attackSpeed -= 0.5f;
            maxTime = attackSpeed;
            brodeningOverlapValue++;
            yield return StartCoroutine(MobSkillManager.instance.Broadening_NightMare());
        }
    }

    public IEnumerator Fear()
    {
        attack += 1f;
        yield return StartCoroutine(MobSkillManager.instance.Fear());
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
        }
        HpTextUpdate();
    }

    public void ConnectInspector()
    {
        BattleGameManager.instance.SetMonster(this.gameObject);
        BattleMagicManager.instance.SetMonster(this);
        MobSkillManager.instance.SetMonster(this);
        battleBuffDebuff = gameObject.AddComponent<BattleBuffManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BattlePlayer>();
    }

    public void FadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GetComponent<SpriteRenderer>().DOFade(1, 1f))
            .OnComplete(() =>
            {
                StartCoroutine(GetGaugeUI("HpBar"));
                hp = maxHp;
                EntitiesStateChange(false);
            });

    }
}
