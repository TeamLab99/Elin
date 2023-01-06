using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Inst { get; private set; }
    void Awake() => Inst = this;
    
    [SerializeField] Image effect;
    Sequence mySequence;
    Sequence mySequence2;

    // Start is called before the first frame update



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

    public void MoveTransform(GameObject obj, PRS prs, bool useDotween, float dotweenTime = 0)
    {
        SetColor("r");
        if (useDotween)
        {
            mySequence = DOTween.Sequence();
            mySequence.Append(obj.transform.DOMove(prs.pos, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1))
            .Join(obj.transform.DORotateQuaternion(prs.rot, dotweenTime))
            .Join(obj.transform.DOScale(prs.scale, dotweenTime).SetRelative().SetEase(Ease.Flash, 2, -1));

            mySequence2 = DOTween.Sequence();
            mySequence2.PrependInterval(dotweenTime/3)
            .Append(effect.DOFade(1, 0.2f))
            .Append(effect.DOFade(0, 0.2f));
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

}
