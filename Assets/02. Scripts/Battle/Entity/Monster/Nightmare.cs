using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EMonsterSkill
{
    Random,
    Broadening,
    Fear,
    Valley,
    Rush
}

public class Nightmare : Monster
{
    private bool isStun;
    private bool isDrain;
    private bool isWall;
    private int drainStack;
    private int brodeningOverlapValue;
    [SerializeField] private EMonsterSkill _monsterSkill;

    public override void Init()
    {
        delay = new WaitForSeconds(Time.deltaTime);
        maxTime = attackSpeed;
        curTime = maxTime;
        count = skillCount;
        hp = maxHp;

    }

    public void SetBattle()
    {
        Init();
        ConnectInspector();
        StartCoroutine(SetState());

        //DialogueManager.instance.NextEricaDialouge("Erica");
    }

    protected override IEnumerator MonsterPattern(int skillCount)
    {
        if (isDrain)
        {
            yield return StartCoroutine(Drain());
            curTime = maxTime;
            count = skillCount;
            yield return null;
        }


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
        ChangeAnim(EMonsterState.Attack);
        EntitiesStateChange(true);
        //gameObject.transform.DOScale(Vector3.one, 0.5f).SetRelative().SetEase(Ease.Flash, 2, 0);

        if (isWall == true)
        {
            if (attackSpeed > 1)
                attackSpeed -= 0.5f;
            maxTime = attackSpeed;
        }


        yield return StartCoroutine(MobSkillManager.instance.CallNormalAttackEffect(1));

        Attack(player);
        if (isStun == false)
            EntitiesStateChange(false);
        else
        {
            GaugeControl(false);
        }

        IconAnimationControl();

        ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator Drain()
    {
        ChangeAnim(EMonsterState.Skill);
        EntitiesStateChange(true);

        yield return StartCoroutine(MobSkillManager.instance.Drain());

        if (isStun == false)
            EntitiesStateChange(false);
        else
        {
            GaugeControl(false);
        }

        IconAnimationControl();

        ChangeAnim(EMonsterState.Idle);
    }

    public IEnumerator Skill()
    {
        ChangeAnim(EMonsterState.Skill);
        EntitiesStateChange(true);

        yield return StartCoroutine(RandomLogic());

        if (isStun == false)
            EntitiesStateChange(false);
        else
        {
            GaugeControl(false);
        }

        ChangeAnim(EMonsterState.Idle);
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

    public IEnumerator Rush()
    {
        yield return StartCoroutine(MobSkillManager.instance.Rush());
    }

    public IEnumerator Valley()
    {
        CardManager.instance.CardsCostUp();
        yield return StartCoroutine(MobSkillManager.instance.Valley());
    }

    public IEnumerator StunPlayer(float time = 1f)
    {
        isStun = true;
        CardManager.instance.DontUseCard(true);
        yield return new WaitForSeconds(time);
        CardManager.instance.DontUseCard(false);
        isStun = false;
    }

    public void IconAnimationControl()
    {
        iconAnimation.Animation(1);
    }

    public override void TakeDamage(float value)
    {
        if (hp - value <= maxHp * 0.4)
        {
            isDrain = false;
            //넘을 수 없는 벽 시작
            isWall = true;
            isStun = true;
            CardManager.instance.DontUseCard(true);
        }
        else if (hp - value <= maxHp * 0.7)
        {
            hp -= value;
            isDrain = true;
        }
        else if (hp - value > 0)
        {
            hp -= value;
            isDrain = false;
        }
        else if (hp - value <= 0)
        {
            hp = 0;
        }

        HpTextUpdate();
    }

    public void ConnectInspector()
    {
        BattleManager.instance.SetMonster(this.gameObject);
        MagicManager.instance.SetMonster(this);
        MobSkillManager.instance.SetMonster(this);
        buffDebuff = gameObject.AddComponent<BuffManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        EntitiesStateChange(true);
        
        StartCoroutine(GetGaugeUI("HpBar"));
    }
    
    public override IEnumerator GetGaugeUI(string tagName)
    {
        yield return new WaitForEndOfFrame();
        var monsterUI = GameObject.FindGameObjectWithTag("MonsterUI").GetComponent<MonsterUIContorller>();
        hpBar = monsterUI.hpBar;
        hpTMP = monsterUI.hpTMP;
        gauge = monsterUI.gauge;
        iconAnimation = monsterUI.gaugeIcon;

        iconAnimation.SetIcon(gaugeIcon);
        iconAnimation.Animation(maxTime);

        HpTextUpdate();
    }

    public IEnumerator SetState()
    {
        yield return new WaitForSeconds(2f);
        gauge.fillAmount = 1f;
    }

    public void BattleStart()
    {
        StartCoroutine(GaugeTimer());
        EntitiesStateChange(false);
    }

    public void DrainHeal()
    {
        hp = maxHp;
        HpTextUpdate();
    }

    public void SetIsDrain(bool _set)
    {
        isDrain = _set;
    }

    public void AttackSpeedDown()
    {
        isWall = true;
        isStun = true;
    }

    IEnumerator RandomLogic()
    {
        if (_monsterSkill == EMonsterSkill.Random)
        {
            switch (Random.Range(1, 5))
            {
                case 1:
                    yield return StartCoroutine(Broadening());
                    break;
                case 2:
                    yield return StartCoroutine(Fear());
                    break;
                case 3:
                    yield return StartCoroutine(Valley());
                    break;
                case 4:
                    yield return StartCoroutine(Rush());
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (_monsterSkill)
            {
                case EMonsterSkill.Broadening:
                    yield return StartCoroutine(Broadening());
                    break;
                case EMonsterSkill.Fear:
                    yield return StartCoroutine(Fear());
                    break;
                case EMonsterSkill.Valley:
                    yield return StartCoroutine(Valley());
                    break;
                case EMonsterSkill.Rush:
                    yield return StartCoroutine(Rush());
                    break;
                default:
                    break;
            }
        }
    }
}