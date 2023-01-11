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
    // 인스펙터
    [SerializeField] TMP_Text healthTMP;

    public int attack;
    public int health;

    public PRS originPRS; // 기존 PRS 저장
    public Vector3 originPos; // 위치값만 저장

    void Start()
    {
        // 초기화
        SetHealth();
        originPos = originPRS.pos;
    }

    // 텍스트 업데이트
    public void SetHealth()
    {
        healthTMP.text = health.ToString();
    }
}
