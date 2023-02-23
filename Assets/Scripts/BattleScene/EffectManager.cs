using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 임시 이펙트 매니저
/// </summary>
public class EffectManager : MonoBehaviour
{
    // 싱글턴
    public static EffectManager Inst { get; private set; }
    void Awake() => Inst = this;
    
    // 인스펙터
    GameObject skillEffect;

    // DOTween용 sequence
    Sequence mySequence;

    // 카드 이동 애니메이션과 다른 점은 오브젝트를 따로 파라미터로 받아서 이동
    public void HitMotion(GameObject obj, bool useDotween, float dotweenTime = 0)
    {
        var p = obj.transform.position + Vector3.left * 27;
        var r = Utils.QI;
        var s = Vector3.one;

        if (useDotween)
        {
            // 오브젝트 시퀀스
            mySequence = DOTween.Sequence();
            mySequence.Append(obj.transform.DOMove(p, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1)) // 앞으로 갔다가 돌아옴
            .Join(obj.transform.DORotateQuaternion(r, dotweenTime))
            .Join(obj.transform.DOScale(s, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1)); // 커졌다가 돌아옴
        }
        else
        {
            // 위치만 바꾸기
            transform.position = p;
            transform.rotation = r;
            transform.localScale = s;
        }
    }

    public void MonsterSkillEffectOn()
    {
        skillEffect.SetActive(true);
    }

    public void SetSkillEffect(GameObject obj)
    {
        skillEffect = obj;
    }
}
