using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class BattleCardManager : Singleton<BattleCardManager>
{

    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardHandTransform;
    [SerializeField] Transform cardSpawnTrasnform;
    [SerializeField] Transform cardUseTrasnform;
    [SerializeField] Transform cardLeft;
    [SerializeField] Transform cardRight;
    [SerializeField] List<BattleCard> myCards;
    [SerializeField] ECardState eCardState;
    [SerializeField] int maxCost;
    [SerializeField] float costRecoverySpeed;
    [SerializeField] MagicSO magicSO;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] TMP_Text maxCostTMP;

    Dictionary<int, DeckCard> deckCards;
    Dictionary<int, UnlockCard> unlockCards;
    List<DeckCard> shuffleCards;
    string[] keyArray = { "A", "S", "D", "F", "G" };
    BattleCard selectCard;
    int currentCardNumber = -1;
    enum ECardState { Loading, CanUseCard, ActivatingCard, Noting }
    bool isSelected;
    bool isCardActivating;
    bool isMonsterAttack;

    int cost;
    float curCostTIme;

    // 카드 사용 시 몬스터에게 알려줄 이벤트
    public static Action<bool> EffectPlayBack;

    public IEnumerator GetCost()
    {
        yield return new WaitForEndOfFrame();
        var costImg = GameObject.FindGameObjectWithTag("Cost").GetComponentsInChildren<TMP_Text>();

        costTMP = costImg[0];
        maxCostTMP = costImg[1];
        TextUpdate();
    }

    public void SetStat(float maxCost, float costRecovery)
    {
        this.maxCost = (int)maxCost;
        costRecoverySpeed = costRecovery;
    }

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
        BattleTurnManager.OnAddCard += AddCard;
        cost = maxCost;
        curCostTIme = costRecoverySpeed;
        //BattleTurnManager.EndDrawPhase += EndDrawPhase;
    }

    void OnDestroy()
    {
        BattleTurnManager.OnAddCard -= AddCard;
        //BattleTurnManager.EndDrawPhase -= EndDrawPhase;
    }


    public void CreatePoolCard()
    {
        Managers.Pool.CreatePool(cardPrefab, 10);
    }

    void TextUpdate()
    {
        maxCostTMP.text = maxCost.ToString();
        costTMP.text = cost.ToString();
    }

    void AddCard()
    {
        //var cardGeneratePositon = cardsParentTransform.position + Vector3.up * -4.5f;

        var cardObject = Managers.Pool.Pop(cardPrefab, cardHandTransform);
        cardObject.transform.position = cardSpawnTrasnform.position;

        var card = cardObject.GetComponent<BattleCard>();
        card.Setup(PopCard());
        card.SetImage(magicSO.items[card.deckCard.index-1].cardImage);
        myCards.Add(card);

        SetOriginOrder();
        StartCoroutine(CardAlignment());
    }

    void SetOriginOrder()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(myCards.Count - i);
        }
    }

    /*    void EndDrawPhase()
        {
            // 5장 드로우가 끝나고 실행될 함수
        }*/

    void Update()
    {
        SetEcardState();
        InputKey();

        if (cost < maxCost)
        {
            curCostTIme -= Time.deltaTime;

            if (curCostTIme <= 0)
            {
                cost++;
                TextUpdate();
                curCostTIme = costRecoverySpeed;
            }
        }

    }

    void InputKey()
    {
        if (eCardState == ECardState.CanUseCard)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ChoiceCard(0);
            }
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

            if (Input.GetKeyDown(KeyCode.Space) && !isMonsterAttack)
            {
                if (isSelected && cost - selectCard?.deckCard.cost > -1)
                    StartCoroutine(UseCard());
            }
        }
    }

    void ChoiceCard(int index)
    {
        if (index >= myCards.Count)
            return;

        if (selectCard == myCards[index])
        {
            SelectAndEnlarge(selectCard, false);
            return;
        }
        else
        {
            if (selectCard != null)
                EnlargeCard(false, selectCard);

            SelectAndEnlarge(myCards[index], true);
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
            SelectAndEnlarge(myCards[0], true);
        }
        else
        {
            var trueNum = currentCardNumber + arrowDirection;

            if (trueNum < 0)
            {
                SelectAndEnlarge(myCards[myCards.Count - 1], true);
                return;
            }
            else if (trueNum >= myCards.Count)
            {
                SelectAndEnlarge(myCards[0], true);
                return;
            }

            SelectAndEnlarge(myCards[trueNum], true);
        }
    }

    void SelectAndEnlarge(BattleCard card, bool isSelecting)
    {
        EnlargeCard(true, card);
        selectCard = card;
        currentCardNumber = myCards.FindIndex(x => x == card);
    }

    public void EnlargeCard(bool isEnlarge, BattleCard card)
    {

        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(316.7f, -5.25f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 0.25f), true, 0.15f);
            isSelected = isEnlarge;
            OtherCardsMove(card, true);
        }
        else
        {
            card.MoveTransform(card.originPRS, true, 0.15f);
            isSelected = isEnlarge;
            OtherCardsMove(card, false);
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);

    }

    void OtherCardsMove(BattleCard card, bool isMove)
    {
        if (isSelected == isMove)
            return;

        int index = myCards.FindIndex(x => x == card);

        if (isMove)
        {
            if (index == myCards.Count - 1)
            {
                for (int i = 0; i < index; i++)
                {
                    myCards[i].MoveTransform(new PRS(myCards[i].originPRS.pos + Vector3.right * -1f, myCards[i].originPRS.rot, Vector3.one * 0.1f), true, 0.15f);
                }
            }
            else if (index == 0)
            {
                for (int i = 1; i < myCards.Count; i++)
                {
                    myCards[i].MoveTransform(new PRS(myCards[i].originPRS.pos + Vector3.right * 1f, myCards[i].originPRS.rot, Vector3.one * 0.1f), true, 0.15f);
                }
            }
            else
            {
                for (int i = index + 1; i < myCards.Count; i++)
                {
                    myCards[i].MoveTransform(new PRS(myCards[i].originPRS.pos + Vector3.right * 1f, myCards[i].originPRS.rot, Vector3.one * 0.1f), true, 0.15f);
                }

                for (int i = index - 1; i >= 0; i--)
                {
                    myCards[i].MoveTransform(new PRS(myCards[i].originPRS.pos + Vector3.right * -1f, myCards[i].originPRS.rot, Vector3.one * 0.1f), true, 0.15f);
                }
            }
        }
        else
        {
            for (int i = 0; i < myCards.Count; i++)
            {
                myCards[i].MoveTransform(myCards[i].originPRS, true, 0.15f);
            }
        }

    }

    IEnumerator CardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(cardLeft, cardRight, myCards.Count, 1f, Vector3.one * 0.35f);

        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.35f);
        }
        yield return null;
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

            if (objCount == 3)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve; //위에서 높이에 제곱을 해버리면 무조건 양수기 때문에 높이가 음수라면 커브도 음수로 변경
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]); //구형을 그리면서 Lerp

                if (i == 0 || i == 2)
                {
                    targetPos.y -= 0.75f;
                }
                else
                {
                    targetPos.y -= 0.5f;
                }
            }
            else if (objCount == 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);

                if (i == 0 || i == 3)
                {
                    targetPos.y -= 0.75f;
                }
                else
                {
                    targetPos.y -= 0.4f;
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
                    targetPos.y -= 0.8f;
                }
                else if (i == 1 || i == 3)
                {
                    targetPos.y -= 0.49f;
                }
                else
                {
                    targetPos.y -= 0.35f;
                }
            }
            else if (objCount <= 2)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve; //위에서 높이에 제곱을 해버리면 무조건 양수기 때문에 높이가 음수라면 커브도 음수로 변경
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]); //구형을 그리면서 Lerp

                targetPos.y -= 0.6f;
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    IEnumerator UseCard()
    {
        if (selectCard == null)
            yield break;
        isCardActivating = true;
        EnlargeCard(true, selectCard);
        EffectPlayBack?.Invoke(true);
        myCards.Remove(selectCard);


        yield return StartCoroutine(selectCard.MoveTransformCoroutine(new PRS(cardUseTrasnform.position, Utils.QI, Vector3.one * 0.35f), true, 0.5f));
        BattleMagicManager.instance.CallMagic(selectCard.deckCard);
        selectCard.DOKill();
        Managers.Pool.Push(selectCard.GetComponent<Poolable>());

        if (myCards.Count == 1)
            currentCardNumber = -1;
        else
            currentCardNumber -= 1;

        if (myCards.Count > 0)
        {
            StartCoroutine(CardAlignment());
            SetKey();
        }
        else
        {
            StartCoroutine(BattleTurnManager.instance.ReDrawCards());
        }

        cost -= selectCard.deckCard.cost;
        TextUpdate();

        isCardActivating = false;
        EffectPlayBack?.Invoke(false);
        selectCard = null;
    }

    public void SetKey()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].SetKey(keyArray[i]);
        }
    }

    public void DontUseCard(bool isBool)
    {
        isMonsterAttack = isBool;
    }

    void SetEcardState()
    {
        if (BattleTurnManager.instance.isLoading)
            eCardState = ECardState.Loading;

        else if (isCardActivating)
            eCardState = ECardState.ActivatingCard;

        else if (myCards.Count > 0 && !BattleTurnManager.instance.isLoading)
            eCardState = ECardState.CanUseCard;

        else
            eCardState = ECardState.Noting;
    }

    public void SetActive(bool isOn)
    {
        gameObject.SetActive(isOn);
    }
}
