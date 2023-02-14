using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using DG.Tweening;

/// <summary>
/// 카드 리스트를 관리하는 매니저 스크립트
/// 이동, 정렬, 선택, 사용 등 카드와 관련된 모든 기능들을 관리한다.
/// </summary>

public class CardManager : MonoBehaviour
{
    // 싱글턴
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    #region 인스펙터
    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform cardUsePoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] TMP_Text playerCostTMP;
    [SerializeField] Player player;
    //[SerializeField] ECardState eCardState;
    #endregion
    
    // 뽑은 카드들의 정보를 담고 있는 리스트
    List<Item> itemBuffer;

    // 카드 선택 관련
    Card selectCard;
    bool isFirstSelect = false;
    bool isCardMoving = false;
    bool isCardUsed = false;
    bool isEnlarge = false;
    bool isNumpad = false;
    float maxTime = 1f;
    static float curTime=0;

    int playerCost =5;
    int curCardNum = 0;
    int prevCardNum = 0;
    WaitForSeconds delay02 = new WaitForSeconds(0.2f);

    //WaitForSeconds delay015 = new WaitForSeconds(0.15f);
    //enum ECardState { Loading, CanSelectCard, CanUseCard }

    // 모든 카드의 정보를 itemBuffer에 입력하고 랜덤으로 섞기
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

    // ItemBuffer에서 제일 앞에 있는 카드 뽑기
    public Item PopItem()
    {
        if (itemBuffer.Count == 0)
            SetupItemBuffer();

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0); // 뽑고 삭제
        return item;
    }

    private void Start()
    {
        // 카드 정보 가져오고 섞고, 첫 드로우
        SetupItemBuffer();
        TurnManager.OnAddCard += AddCard;
        curTime = maxTime;
    }

    private void OnDestroy()
    {
        TurnManager.OnAddCard -= AddCard;
    }

    private void Update()
    {
        // 현재 카드 개수가 0이 될 시 다시 5장 드로우
        if (!TurnManager.Inst.isLoading && myCards.Count <= 0)
        {
            StartCoroutine(TurnManager.Inst.CardDraw());
        }

        if (playerCost>= 0 && playerCost < 5)
        {
            // Time.DeltaTime을 이용하여 curTime이 0초가 될 때 마다 몬스터의 공격 함수 호출
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                // 게이지 재충전, 어택 애니메이션, 공격 함수
                curTime = maxTime;
                playerCost++;
                playerCostTMP.text = playerCost + "/5";

            }
        }

        //SetECardState();
    }

    // 카드 한 장 드로우
    void AddCard(bool isMine)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI); // 프리팹을 토대로 카드 오브젝트 생성
        var card = cardObject.GetComponent<Card>(); // 오브젝트의 카드 가져옴
        card.Setup(PopItem()); // 랜덤으로 섞인 아이템 리스트에서 하나를 뽑아서 데이터 입력
        myCards.Add(card); // 카드 리스트에 추가

        // 레이어 순서 바꾸기, 정렬, 키 네임 셋팅 
        SetOriginOrder(true);
        CardAlignment(true, 0.7f);
        SetKeyName();
    }

    #region 카드 정렬
    // Order 정렬
    private void SetOriginOrder(bool isMine)
    {
        int count = myCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    // 카드 위치 정렬
    void CardAlignment(bool isMine, float time)
    {
        // 카드들의 기존 위치를 닮고 있는 리스트 만들고 RoundAlignment 계산 한다음 위치 재 입력

        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 2f);

        var targetCards = myCards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, time);
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
    #endregion

    #region 카드
    // 선택한 카드 사용
    public IEnumerator TryPutCardCorutine()
    {
        if (!isCardMoving && isEnlarge) // 다른 카드가 안 움직이고 있고, 이 카드가 선택된 상태라면
        {
            if (playerCost - selectCard.GetCost() >= 0)
            {
                playerCost--;
                playerCostTMP.text = playerCost + "/5";
                // 카드 리스트에서 카드 삭제 후 정렬 먼저 함
                Card card = selectCard;
                myCards.Remove(selectCard);
                SetKeyName();
                CardAlignment(true, 0.5f);

                selectCard = null;
                isEnlarge = false;
                isCardMoving = false;
                isCardUsed = true;

                // 카드 애니메이션
                card.MoveTransform(new PRS(cardUsePoint.position, Utils.QI, Vector3.one * 2.5f), true, 0.4f);
                yield return delay02;
                card.FadeOut(0.4f);
            }
            else
            {
                yield return null;
            }

        }
        else
        {
            yield return null;
        }
    }

    // 카드 사용
    public void UseCard(Item item)
    {
        if (item.type == "공격")
        {
            // 적 체력 감소
            Battle.Inst.PlayerAttack();
        }
        else if (item.type == "회복")
        {
            // 자신 체력 회복
            Battle.Inst.PlayerHeal();
        }
    }

    // 카드 선택 시 위로 살짝 이동 및 확대 애니메이션
    public IEnumerator EnlargeCard(bool isEnlarge, int cardNum)
    {
        if (isEnlarge)
        {
            // 이동 애니메이션 없이 위치 값만 변경
            Vector3 enlargePos = new Vector3(myCards[cardNum].originPRS.pos.x, -9.65f, -15f); //카드 겹쳐서 선택될까봐 z위치도 -10으로 땡겨줌
            myCards[cardNum].MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 2.5f), false);
            this.isEnlarge = true;
            selectCard = myCards[cardNum];
        }
        else
        {
            // 원래 자리로 돌아가는 애니메이션
            myCards[cardNum].MoveTransform(myCards[cardNum].originPRS, true, 0.1f);
            this.isEnlarge = false;
            selectCard = null;
        }

        // 이후 레이어 순서 변경
        myCards[cardNum].GetComponent<Order>().SetMostFrontOrder(isEnlarge);
        yield return null;
    }

    // 키패드 선택
    public IEnumerator MoveToChoiceNum(int curNum)
    {
        if (!isCardMoving && myCards.Count > curNum) // 안 움직이고 있고 선택한 카드 번호(순서)가 패에 존재하는지
        {
            isCardMoving = true;

            NumpadCheck(); // 방향키 이동과 충돌 방지

            curCardNum = curNum;

            // 전투 시작하고 카드를 처음 선택하는건지 판단
            if (!isFirstSelect)
                isFirstSelect = true;

            // 이전에 이미 선택된 카드 있을 시 제자리로 돌려보냄
            if (isEnlarge && myCards.Count != 1 && prevCardNum != curCardNum)
                yield return StartCoroutine(EnlargeCard(false, prevCardNum));

            // 그리고 새로 선택된거 확대
            yield return StartCoroutine(EnlargeCard(true, curCardNum));

            prevCardNum = curCardNum;
            isNumpad = true;
            isCardMoving = false;
        }
        else
        {
            yield return null;
        }
    }

    // 방향키 선택
    public IEnumerator MoveToArrow(bool direction)
    {
        if (!isCardMoving && myCards.Count > 0)
        {
            isCardMoving = true;

            NumpadCheck();

            if (isEnlarge && myCards.Count != 1)
                yield return StartCoroutine(EnlargeCard(false, curCardNum));
            
            if (!isFirstSelect)
            {
                // 첫 선택을 방향키로 했을 시 가운데부터 선택
                curCardNum = 2;
                isFirstSelect = true;
            }
            else
            {
                if (direction)
                {
                    // 우 방향키 일 때
                    if (curCardNum < myCards.Count - 1)
                        curCardNum++;
                    else if (curCardNum <= myCards.Count - 1)
                        curCardNum = 0;
                }
                else
                {
                    //좌 방향키 일 때
                    if (curCardNum > 0)
                        curCardNum--;
                    else if (curCardNum <= 0)
                        curCardNum = myCards.Count - 1;
                }
            }

            if (isCardUsed)
            {
                // 카드를 패에서 냈을 때 다음 선택 카드 고정
                switch (myCards.Count)
                {
                    case 1:
                    case 2:
                        curCardNum = 0;
                        break;
                    case 3:
                    case 4:
                        curCardNum = 1;
                        break;
                }
                isCardUsed = false;
            }

            // 카드 확대
            yield return StartCoroutine(EnlargeCard(true, curCardNum));

            yield return delay02;

            prevCardNum = curCardNum;
            isNumpad = false;
            isCardMoving = false;

            // 키를 꾹 눌렀을 때 계속 이동하게 하기 위해서 재귀
            StartCoroutine(MoveToArrow(direction));
        }
        else
        {
            yield return null;
        }
    }

    // 모든 코루틴 멈추는 함수
    public void StopCo()
    {
        StopAllCoroutines();
        isCardMoving = false;
    }

    // 카드 선택 취소
    public void AllEnlargeCancle()
    {
        if (!isCardMoving && isEnlarge)
        {
            for (int i = 0; i < myCards.Count; i++)
                StartCoroutine(EnlargeCard(false, i));
        }
        else
            return;
    }

    // 키패드, 방향키 충돌 방지
    public void NumpadCheck()
    {
        if (!isNumpad)
        {
            prevCardNum = curCardNum;
        }
        else
        {
            curCardNum = prevCardNum;
        }
    }
    #endregion

    // isCardMoving 캡슐화
    public void SetIsCardMoving(bool isCardMoving)
    {
        this.isCardMoving = isCardMoving;
    }

    // 카드 선택 키 텍스트 설정
    void SetKeyName()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            switch (i)
            {
                case 0:
                    myCards[i].SetKey("A");
                    break;
                case 1:
                    myCards[i].SetKey("S");
                    break;
                case 2:
                    myCards[i].SetKey("D");
                    break;
                case 3:
                    myCards[i].SetKey("F");
                    break;
                case 4:
                    myCards[i].SetKey("G");
                    break;
            }
        }
    }

    /*    private void SetECardState()
        {
            if (TurnManager.Inst.isLoading)
                eCardState = ECardState.Loading;

            else if (!TurnManager.Inst.myTurn || myPutCount == 1 || EntityManager.Inst.IsFullMyEntities)
                eCardState = ECardState.CanSelectCard;

            else if (TurnManager.Inst.myTurn && myPutCount == 0)
                eCardState = ECardState.CanUseCard;
        }*/
}
