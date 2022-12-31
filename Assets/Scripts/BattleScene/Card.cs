using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    [SerializeField] SpriteRenderer character;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] Sprite cardFront;
    [SerializeField] Sprite cardBack;

    public Item item;
    bool isFront;
    public PRS originPRS;
    Sequence mySequence;
    Sequence mySequence2;

    public void Setup(Item item, bool isFront)
    {
        this.item = item;
        this.isFront = isFront;

        if (this.isFront)
        {
            character.sprite = this.item.sprite;
            nameTMP.text = this.item.name;
            attackTMP.text = this.item.attack.ToString();
            healthTMP.text = this.item.health.ToString();
        }
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOMove(prs.pos, dotweenTime))
            .Join(transform.DORotateQuaternion(prs.rot, dotweenTime))
            .Join(transform.DOScale(prs.scale, dotweenTime));
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    public void FadeInOut(float dotweenTime = 0)
    {
        mySequence2 = DOTween.Sequence();
        mySequence2
            .Append(transform.GetComponent<SpriteRenderer>().DOFade(0, dotweenTime))
            .Join(character.GetComponent<SpriteRenderer>().DOFade(0, dotweenTime))
            .Join(nameTMP.DOFade(0, dotweenTime))
            .Join(healthTMP.DOFade(0, dotweenTime))
            .Join(attackTMP.DOFade(0, dotweenTime))
            .OnComplete(() =>
            {
                BPGameManager.Inst.isDelay = false;
                BPGameManager.Inst.isFirstSelect = false;
                DestroyImmediate(gameObject);
            });
    }


    public void OnDestroy()
    {
        transform.DOKill();
        character.DOKill();
    }
}
