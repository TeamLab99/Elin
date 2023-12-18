using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer cardSprite;
    [SerializeField] SpriteRenderer magicSprite;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text keyTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] GameObject costUpEffect;
    
    public DeckCard deckCard;
    public PRS originPRS;
    public int currentCost;
    
    public void Setup(DeckCard deckCard)
    {
        this.deckCard = deckCard;
        currentCost=this.deckCard.cost;
        // 데이터 베이스 이미지 연결 선행 작업 필요
        // magicSprite.sprite = deckCard.image
        nameTMP.text = deckCard.cardName;
        costTMP.text = currentCost.ToString();
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

    public IEnumerator MoveTransformCoroutine(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        keyTMP.text = "";
        if (useDotween)
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOMove(prs.pos, dotweenTime))
                .Join(transform.DORotateQuaternion(prs.rot, dotweenTime))
                .Join(transform.DOScale(0f, dotweenTime))
                .Join(GetComponent<SpriteRenderer>().DOFade(0, dotweenTime))
                .Join(nameTMP.GetComponent<TMP_Text>().DOFade(0, dotweenTime))
                .Join(costTMP.GetComponent<TMP_Text>().DOFade(0, dotweenTime));
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }

        yield return new WaitForSeconds(dotweenTime);
    }

    public void SetKey(string keycode)
    {
        keyTMP.text = keycode;
    }

    public void SetImage(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void OnDisable()
    {
        GetComponent<SpriteRenderer>().color =Color.white;
        nameTMP.GetComponent<TMP_Text>().color = Color.white;
        costTMP.GetComponent<TMP_Text>().color = Color.white;
    }

    public void CostUp()
    {
        StartCoroutine(StackUpEffect());
        currentCost++;
        costTMP.text = currentCost.ToString();
    }

    IEnumerator StackUpEffect()
    {
        costUpEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        costUpEffect.SetActive(false);
    }
}