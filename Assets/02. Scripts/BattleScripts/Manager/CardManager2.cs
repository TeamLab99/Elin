using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardManager2 : Singleton<CardManager2>
{
    Dictionary<int, DeckCard> deckCards;
    Dictionary<int, UnlockCard> unlockCards;
    List<DeckCard> shuffleCards;

    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardHandTransform;
    [SerializeField] Transform cardSpawnTrasnform;
    [SerializeField] Transform cardLeft;
    [SerializeField] Transform cardRight;
    [SerializeField] List<Card2> myCards;

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
    }

    void OnDestroy()
    {
        TurnManager2.OnAddCard -= AddCard;
    }

    void Update()
    {
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
            case 3: objLerps = new float[] { 0.1f, 0.5f ,0.9f}; break;
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
}
