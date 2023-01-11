using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{

    // 카드 프리팹에 담긴 Object들 가져오기
    [SerializeField] SpriteRenderer card;
    [SerializeField] SpriteRenderer character;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text keyTMP;
    [SerializeField] TMP_Text typeTMP;

    // 카드에 담긴 정보, 초기 위치
    public Item item;
    public PRS originPRS;

    // 애니메이션 재생을 위한 시퀀스
    Sequence mySequence;
    Sequence mySequence2;

    // 카드에 정보 Setup
    public void Setup(Item item)
    {
        this.item = item;
        character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        attackTMP.text = this.item.attack.ToString();
        healthTMP.text = this.item.health.ToString();
        typeTMP.text = this.item.type;

        // 회복 타입이면 타입 텍스트 초록색으로 변경
        if (this.item.type == "회복")
        {
            typeTMP.color = new Color32(0, 255, 0, 255);
        }
    }
    // 카드 이동 애니메이션 PRS, DOTween 사용여부, 총 재생 시간을 파라미터로 받아와 실행
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
            // 애니메이션 없이 위치만 바뀜
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    // 카드 사용 시 FadeOut 애니메이션
    public void FadeOut(float dotweenTime = 0)
    {
        mySequence2 = DOTween.Sequence();
        mySequence2
            .Append(transform.GetComponent<SpriteRenderer>().DOFade(0, dotweenTime))
            .Join(character.GetComponent<SpriteRenderer>().DOFade(0, dotweenTime))
            .Join(nameTMP.DOFade(0, dotweenTime))
            .Join(healthTMP.DOFade(0, dotweenTime))
            .Join(attackTMP.DOFade(0, dotweenTime))
            .Join(keyTMP.DOFade(0, dotweenTime))
            .Join(typeTMP.DOFade(0, dotweenTime))
            .OnComplete(() =>
            {
                    // 시퀀스가 끝났을 때 카드 효과를 발동시키고 게임오브젝트를 파괴시킴. -> 남아있는 카드들의 정렬을 위해
                    CardUse();
                DestroyImmediate(gameObject);
            });
    }

    // 카드들 선택 키 보여주기
    public void SetKey(string str)
    {
        keyTMP.text = str;
    }

    // 카드 사용
    void CardUse()
    {
        CardManager.Inst.UseCard(this.item);
    }
}
