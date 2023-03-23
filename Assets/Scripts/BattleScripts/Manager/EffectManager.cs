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
    public void MobAtkMotion(GameObject mob, float dotweenTime = 0)
    {
        var s = Vector3.one * 0.7f;
        mob.transform.DOScale(s, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1); // 커졌다가 돌아옴
    }

    public void HeadButtMotion(GameObject obj, float dotweenTime = 0)
    {
        mySequence = DOTween.Sequence();
        mySequence.Append(obj.transform.DOMoveX(15, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, 0))
            .Join(obj.transform.DOScale(Vector3.one, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, 0));
    }

    public IEnumerator HitMotion(GameObject mob)
    {
        var spr = mob.GetComponent<SpriteRenderer>();

        mySequence = DOTween.Sequence();
        mySequence
            .Append(spr.DOFade(0.5f, 0.1f))
            .Append(spr.DOFade(1f, 0.1f))
            .Append(spr.DOFade(0.5f, 0.1f))
            .Append(spr.DOFade(1, 0.1f));
        yield return null;
    }

    public IEnumerator DeadMotion(SpriteRenderer spr)
    {
        spr.color = new Color32(255, 0, 0, 100);
        spr.DOFade(0, 2f);
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
