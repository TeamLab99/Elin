using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
/// 캐릭터의 정보를 담을 Entity 스크립트
/// 현재 체력의 정보만 담겨져 있으며, 상속을 통해 확장시킬 예정이다.
/// </summary>
public class Entity : MonoBehaviour
{
    [SerializeField] SpriteRenderer characterSprite;
    [SerializeField] TMP_Text healthTMP;   

    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] float attack;
    [SerializeField] float defense;

    PRS originPRS; // 기존 PRS 저장
    Vector3 originPos; // 위치값만 저장


    public void Attack(Entity entity)
    {
        entity.TakeDamage(attack);
    }

    public void TakeDamage(float amount)
    {
        amount -= defense;

        if (health > 0)
        {
            health -= amount;
        }

        if (health <= 0)
        {
            health = 0;
            Battle.Inst.GameOver();
        }

        healthTMP.text = health.ToString();
    }

    public void Heal(float amount)
    {
        health += amount;

        if (health > maxHealth)
            health = maxHealth;
        healthTMP.text = health.ToString();

    }

    public void HealthUpdate()
    {
        healthTMP.text = health.ToString();
    }


}
