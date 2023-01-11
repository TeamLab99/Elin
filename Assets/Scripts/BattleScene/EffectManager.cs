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
    [SerializeField] Image effect;
    
    // DOTween용 sequence
    Sequence mySequence;
    Sequence mySequence2;

    // Image 색 변경
    public void SetColor(string str)
    {
        switch (str)
        {
            case "r":
                effect.color = new Color32(236, 110, 110, 0);
                break;
            case "g":
                effect.color = new Color32(0, 255, 0, 255);
                break;
            case "b":
                effect.color = new Color32(0, 0, 255, 255);
                break;
        }
    }

    // 카드 이동 애니메이션과 다른 점은 오브젝트를 따로 파라미터로 받아서 이동
    public void MoveTransform(GameObject obj, PRS prs, bool useDotween, float dotweenTime = 0)
    {
        SetColor("r"); // 빨강
        if (useDotween)
        {
            // 오브젝트 시퀀스
            mySequence = DOTween.Sequence();
            mySequence.Append(obj.transform.DOMove(prs.pos, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1)) // 앞으로 갔다가 돌아옴
            .Join(obj.transform.DORotateQuaternion(prs.rot, dotweenTime))
            .Join(obj.transform.DOScale(prs.scale, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1)); // 커졌다가 돌아옴

            // 이미지 시퀀스
            mySequence2 = DOTween.Sequence();
            mySequence2.PrependInterval(dotweenTime/3)
            .Append(effect.DOFade(1, 0.2f)) // FadeIn 됐다가
            .Append(effect.DOFade(0, 0.2f)); //FadeOut 됨
        }
        else
        {
            // 위치만 바꾸기
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

}
