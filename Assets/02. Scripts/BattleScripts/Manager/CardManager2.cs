using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class CardManager2 : Singleton<CardManager2>
{

    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardHandTransform;
    [SerializeField] Transform cardSpawnTrasnform;
    [SerializeField] Transform cardUseTrasnform;
    [SerializeField] Transform cardLeft;
    [SerializeField] Transform cardRight;
    [SerializeField] List<Card2> myCards;
    [SerializeField] ECardState eCardState;

    Dictionary<int, DeckCard> deckCards;
    Dictionary<int, UnlockCard> unlockCards;
    List<DeckCard> shuffleCards;
    string[] keyArray = { "A", "S", "D", "F", "G" };
    Card2 selectCard;
    int currentCardNumber = -1;
    enum ECardState { Loading, CanSelectCard, CanUseCard }
    bool canUseCard = true;

    // 카드 사용 시 몬스터에게 알려줄 이벤트
    //public static event Action UseTheCard;

    public DeckCard PopCard()
    {
        if (shuffleCards.Count == 0)
            SetupCardBuffer();

        DeckCard deckCard = shuffleCards[0];
        shuffleCards.RemoveAt(0);
        return deckCard;
    }

    void SetupCardBuffer()
    {
        shuffleCards = new List<DeckCard>(100);
        deckCards = Managers.Data.CardDict;
        unlockCards = Managers.Data.DeckDict;

        foreach (KeyValuePair<int, DeckCard> deck in deckCards)
        {
            foreach (KeyValuePair<int, UnlockCard> unlock in unlockCards)
            {
                if (deck.Key == unlock.Value.index)
                {
                    for (int i = 0; i < deck.Value.percent; i++)
                    {
                        shuffleCards.Add(deck.Value);
                    }
                }
            }
        }

        for (int i = 0; i < shuffleCards.Count; i++)
        {
            int rand = Random.Range(i, shuffleCards.Count);
            DeckCard temp = shuffleCards[i];
            shuffleCards[i] = shuffleCards[rand];
            shuffleCards[rand] = temp;
        }
    }

    void Start()
    {
        SetupCardBuffer();
        TurnManager2.OnAddCard += AddCard;
        TurnManager2.EndDrawPhase += EndDrawPhase;
    }

    void OnDestroy()
    {
        TurnManager2.OnAddCard -= AddCard;
        TurnManager2.EndDrawPhase -= EndDrawPhase;
    }

    void AddCard()
    {
        //var cardGeneratePositon = cardsParentTransform.position + Vector3.up * -4.5f;
        var cardObject = Instantiate(cardPrefab, cardSpawnTrasnform.position, Utils.QI);
        cardObject.transform.parent = cardHandTransform;

        var card = cardObject.GetComponent<Card2>();
        card.Setup(PopCard());
        myCards.Add(card);

        SetOriginOrder();
        CardAlignment();
    }

    void SetOriginOrder()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void EndDrawPhase()
    {
        // 5장 드로우가 끝나고 실행될 함수
    }

    void Update()
    {
        SetEcardState();
        InputKey();
    }

    void InputKey()
    {
        if (eCardState == ECardState.CanSelectCard && canUseCard)
        {
            if (Input.GetKeyDown(KeyCode.A))
                ChoiceCard(0);
            else if (Input.GetKeyDown(KeyCode.S))
                ChoiceCard(1);
            else if (Input.GetKeyDown(KeyCode.D))
                ChoiceCard(2);
            else if (Input.GetKeyDown(KeyCode.F))
                ChoiceCard(3);
            else if (Input.GetKeyDown(KeyCode.G))
                ChoiceCard(4);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                ChoiceCardWithKeyboard(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                ChoiceCardWithKeyboard(1);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(CardUse());
            }
        }
    }

    void ChoiceCard(int index)
    {
        if (index > myCards.Count)
            return;

        if (selectCard == myCards[index])
        {
            currentCardNumber = index;
            EnlargeCard(true, selectCard);
            return;
        }
        else
        {
            if (selectCard != null)
                EnlargeCard(false, selectCard);

            selectCard = myCards[index];
            currentCardNumber = index;
            EnlargeCard(true, selectCard);
        }
    }

    void ChoiceCardWithKeyboard(int arrowDirection)
    {
        if (myCards.Count <= 0)
            return;

        if (selectCard != null)
            EnlargeCard(false, selectCard);

        if (myCards.Count == 1 || currentCardNumber == -1)
        {
            selectCard = myCards[0];
            EnlargeCard(true, selectCard);
            currentCardNumber = 0;
        }
        else
        {
            var trueNum = currentCardNumber + arrowDirection;

            if (trueNum < 0)
            {
                selectCard = myCards[myCards.Count - 1];
                EnlargeCard(true, selectCard);
                currentCardNumber = myCards.Count - 1;
                return;
            }
            else if (trueNum >= myCards.Count)
            {
                selectCard = myCards[0];
                EnlargeCard(true, selectCard);
                currentCardNumber = 0;
                return;
            }

            selectCard = myCards[trueNum];
            EnlargeCard(true, selectCard);
            currentCardNumber = trueNum;
        }
    }

    private void CardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(cardLeft, cardRight, myCards.Count, 0.5f, Vector3.one * 1);

        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];

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
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    public void EnlargeCard(bool isEnlarge, Card2 card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -6.8f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 1.2f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    public void SetKey()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].SetKey(keyArray[i]);
        }
    }

    IEnumerator CardUse()
    {
        if (selectCard == null)
            yield break;

        canUseCard = false;
        // UseTheCard 이벤트 호출

        myCards.Remove(selectCard);
        if (myCards.Count <= 0)
            StartCoroutine(TurnManager2.instance.ReDrawCards());
        else
        {
            CardAlignment();
            SetKey();
        }
        currentCardNumber = -1;

        yield return StartCoroutine(selectCard.MoveTransformCoroutine(cardUseTrasnform.position, true, 0.5f));

        selectCard.DOKill();
        Destroy(selectCard.gameObject);
        selectCard = null;
        canUseCard = true;
    }

    void SetEcardState()
    {
        if (TurnManager2.instance.isLoading)
            eCardState = ECardState.Loading;
        else if (!TurnManager2.instance.isLoading)
            eCardState = ECardState.CanSelectCard;
    }
}
