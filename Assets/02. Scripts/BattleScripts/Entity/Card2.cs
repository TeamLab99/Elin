using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card2 : MonoBehaviour
{
    [SerializeField] SpriteRenderer cardSprite;
    [SerializeField] SpriteRenderer magicSprite;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text keyTMP;
    [SerializeField] TMP_Text costTMP;

    public DeckCard deckCard;
    public PRS originPRS;

    public void Setup(DeckCard deckCard)
    {
        this.deckCard = deckCard;
        // 데이터 베이스 이미지 연결 선행 작업 필요
        // magicSprite.sprite = deckCard.image
        nameTMP.text = deckCard.cardName;
        costTMP.text = deckCard.cost.ToString();
    }

    public void SetKey(string keycode)
    {
        keyTMP.text = keycode;
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
}
