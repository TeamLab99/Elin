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
    [SerializeField] float attack;
    [SerializeField] TMP_Text hpTMP;
    float hp;

    public BattleBuffManager battleBuffDebuff;
    PRS originPRS; // 기존 PRS 저장
    Vector3 originPos; // 위치값만 저장

    protected virtual void Start()
    {
        InitSetting();
    }

    public void InitSetting()
    {
        battleBuffDebuff = gameObject.AddComponent<BattleBuffManager>();
        originPRS = new PRS(transform.position, transform.rotation, transform.localScale);
        originPos = originPRS.pos;
        hp = maxHp;
        HPTxtUpdate();
    }

    public void Attack(Entity entity, int value)
    {
        entity.TakeDmg(value);
    }

    public void TakeDmg(float amount)
    {
        if (hp - amount <= 0)
        {
            hp = 0;
            Debug.Log("게임 오버");
        }
        else
            hp -= amount;

        HPTxtUpdate();
    }

    public void Heal(float amount)
    {
        if (hp + amount >= maxHp)
            hp = maxHp;
        else
            hp += amount;

        HPTxtUpdate();
    }

    public void HPTxtUpdate()
    {
        hpTMP.text = hp.ToString();
    }
}
