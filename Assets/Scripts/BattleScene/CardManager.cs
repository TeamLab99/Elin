using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    #region 인스펙터

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] List<Card> otherCards;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform cardUsePoint;
    [SerializeField] Transform otherCardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform otherCardLeft;
    [SerializeField] Transform otherCardRight;
    [SerializeField] ECardState eCardState;

    public int myCardsCount = 0;
    public bool alreadyEnlarge = false;

    #endregion

    List<Item> itemBuffer;
    Card selectCard;
    bool isMyCardDrag;
    bool onMyCardArea;
    int myPutCount;
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag }

    public Item PopItem()
    {
        if (itemBuffer.Count == 0)
            SetupItemBuffer();

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    private void SetupItemBuffer()
    {
        itemBuffer = new List<Item>(100); // 미리 캐퍼시티 100으로 용량 설정해주면 메모리 효율적으로 사용
        for (int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
            for (int j = 0; j < item.percent; j++)
                itemBuffer.Add(item);
        }

        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    private void Start()
    {
        SetupItemBuffer();
        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += OnTurnStarted;
    }

    private void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
        TurnManager.OnTurnStarted -= OnTurnStarted;

    }

    void OnTurnStarted(bool myTurn)
    {
        if (myTurn)
            myPutCount = 0;
    }

    private void Update()
    {
        /*        if (isMyCardDrag)
                    CardDrag();*/

        DetectCardArea();
        SetECardState();
    }

    void AddCard(bool isMine)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), true);
        myCards.Add(card);

        myCardsCount++;

        Debug.Log("카드 개수:" + myCardsCount);
        SetOriginOrder(true);
        CardAlignment(true);
    }

    private void SetOriginOrder(bool isMine)
    {
        int count = isMine ? myCards.Count : otherCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void CardAlignment(bool isMine)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        if (isMine)
            originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 2f);
        else
            originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one * 2f);

        var targetCards = isMine ? myCards : otherCards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.33f, 0.67f }; break;
            case 3: objLerps = new float[] { 0.16f, 0.5f, 0.84f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]); //중간 값
            var targetRot = Utils.QI;
            if (objCount == 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve; //위에서 높이에 제곱을 해버리면 무조건 양수기 때문에 높이가 음수라면 커브도 음수로 변경
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]); //구형을 그리면서 Lerp

                if (i == 0 || i == 3)
                {
                    targetPos.y -= 0.5f;
                }
            }
            else if (objCount == 5)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve; //위에서 높이에 제곱을 해버리면 무조건 양수기 때문에 높이가 음수라면 커브도 음수로 변경
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]); //구형을 그리면서 Lerp

                if (i == 0 || i == 4)
                {
                    targetPos.y -= 1f;
                }

                if (i == 1 || i == 3)
                {
                    targetPos.y -= 0.5f;
                }
            }
            else if (objCount > 5)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve; //위에서 높이에 제곱을 해버리면 무조건 양수기 때문에 높이가 음수라면 커브도 음수로 변경
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]); //구형을 그리면서 Lerp
            }
            results.Add(new PRS(targetPos, targetRot, scale));

        }
        return results;
    }

    public void TryPutCard()
    {
        BPGameManager.Inst.isDelay = true;

        selectCard.MoveTransform(new PRS(cardUsePoint.position, Utils.QI, Vector3.one * 2.5f),true,0.5f);
        selectCard.FadeInOut(0.4f);

        Invoke("PutCardTimer", 1f);
    }

    public void PutCardTimer()
    {
        BPGameManager.Inst.isDelay = false;

        myCards.Remove(selectCard);
        selectCard.transform.DOKill();
        DestroyImmediate(selectCard.gameObject);
        selectCard = null;
        CardAlignment(true);

    }


    #region MyCard
    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    public void EnlargeCard(bool isEnlarge, int cardNum)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(myCards[cardNum].originPRS.pos.x, -9.65f, -15f); //카드 겹쳐서 선택될까봐 z위치도 -10으로 땡겨줌
            myCards[cardNum].MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 2.5f), false);
            alreadyEnlarge = true;
            selectCard = myCards[cardNum];
        }
        else
        {
            myCards[cardNum].MoveTransform(myCards[cardNum].originPRS, true, 0.15f);
            alreadyEnlarge = false;
            selectCard = null;
        }

        myCards[cardNum].GetComponent<Order>().SetMostFrontOrder(isEnlarge);

    }
    private void SetECardState()
    {
        if (TurnManager.Inst.isLoading)
            eCardState = ECardState.Nothing;

        else if (!TurnManager.Inst.myTurn || myPutCount == 1 || EntityManager.Inst.IsFullMyEntities)
            eCardState = ECardState.CanMouseOver;

        else if (TurnManager.Inst.myTurn && myPutCount == 0)
            eCardState = ECardState.CanMouseDrag;
    }

    #endregion
}
