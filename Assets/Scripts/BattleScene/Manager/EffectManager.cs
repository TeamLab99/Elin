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
    public void AtkMotion(GameObject mob, Vector3 playerPos, bool useDotween, float dotweenTime = 0)
    {
        var p = playerPos;
        var r = Utils.QI;
        var s = Vector3.one * 0.7f;

        if (useDotween)
        {
            // 오브젝트 시퀀스
            mySequence = DOTween.Sequence();
            mySequence.Append(mob.transform.DOMove(p, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1)) // 앞으로 갔다가 돌아옴
            .Join(mob.transform.DORotateQuaternion(r, dotweenTime))
            .Join(mob.transform.DOScale(s, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1)); // 커졌다가 돌아옴
        }
        else
        {
            // 위치만 바꾸기
            transform.position = p;
            transform.rotation = r;
            transform.localScale = s;
        }
    }

    public IEnumerator HitMotion(GameObject mob)
    {
        var spr = mob.GetComponent<SpriteRenderer>();

        mySequence = DOTween.Sequence();
        mySequence
            .Append(spr.DOFade(0.5f,0.1f))
            .Append(spr.DOFade(1f, 0.1f))
            .Append(spr.DOFade(0.5f, 0.1f))
            .Append(spr.DOFade(1,0.1f));
        yield return null;
    }

    public void CallHitCorutine(GameObject obj)
    {
        StartCoroutine(HitMotion(obj));
    }

    public void MobSkillEfc()
    {
        skillEffect.SetActive(true);
    }

    public void SetSkillEfc(GameObject obj)
    {
        skillEffect = obj;
    }
}
