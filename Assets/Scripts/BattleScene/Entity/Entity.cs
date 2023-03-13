using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// 캐릭터의 정보를 담을 Entity 스크립트
/// 현재 체력의 정보만 담겨져 있으며, 상속을 통해 확장시킬 예정이다.
/// </summary>
public class Entity : MonoBehaviour
{
    [SerializeField] float maxHp;
    [SerializeField] float defense;
    [SerializeField] float attack; // 마법 매니저 만들면 몬스터에만 들어갈 속성
    [SerializeField] TMP_Text hpTMP;


    [Header("Pause")]
    protected bool stopGauge;

    float hp;
    SpriteRenderer spr;
    PRS originPRS; // 기존 PRS 저장
    Vector3 originPos; // 위치값만 저장

    protected virtual void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        originPRS = new PRS(transform.position, transform.rotation,
            transform.localScale);
        originPos = originPRS.pos;
        hp = maxHp;
    }

    public void Attack(Entity entity)
    {
        entity.TakeDmg(attack);
    }

    public void TakeDmg(float amount)
    {
        amount -= defense;

        hp -= amount;

        if (hp <= 0)
        {
            hp = 0;
            stopGauge = true;
            StartCoroutine(EffectManager.Inst.DeadMotion(spr));
            StartCoroutine(BPGameManager.Inst.GameOver());
        }

        HPTxtUpdate();
    }

    public void Heal(float amount)
    {
        hp += amount;

        if (hp > maxHp)
            hp = maxHp;

        HPTxtUpdate();
    }

    public void HPTxtUpdate()
    {
        hpTMP.text = hp.ToString();
    }

    public void SetStopGauge(bool isBool)
    {
        stopGauge = isBool;
    }
}
