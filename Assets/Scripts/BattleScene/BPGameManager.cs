using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//치트, UI, 랭킹, 게임오버
public class BPGameManager : MonoBehaviour
{
    [SerializeField] NotificationPanel notificationPanel;
    Color32 red = new Color32(255, 0, 0, 255);
    Color32 yellow = new Color32(255, 244, 0, 255);
    int cardNum = 0;
    int prevNum = 0;
    bool isNumpad = false;
    public bool isCardMoving = false;
    public bool isFirstSelect = false;
    public static BPGameManager Inst { get; private set; }
    void Awake() => Inst = this;
    private void Start()
    {
        StartGame();
    }
    private void Update()
    {
#if UNITY_EDITOR
        InputCheatkey();
#endif
    }
    private void InputCheatkey()
    {
        // 키 입력 판단

        #region 키패드 선택
        if (Input.GetKeyDown(KeyCode.A) && !isCardMoving && CardManager.Inst.myCardsCount > 0)
            NumpadExecution(0);
        if (Input.GetKeyDown(KeyCode.S) && !isCardMoving && CardManager.Inst.myCardsCount > 1)
            NumpadExecution(1);
        if (Input.GetKeyDown(KeyCode.D) && !isCardMoving && CardManager.Inst.myCardsCount > 2)
            NumpadExecution(2);
        if (Input.GetKeyDown(KeyCode.F) && !isCardMoving && CardManager.Inst.myCardsCount > 3)
            NumpadExecution(3);
        if (Input.GetKeyDown(KeyCode.G) && !isCardMoving && CardManager.Inst.myCardsCount > 4)
            NumpadExecution(4);
        #endregion

        #region 방향키 선택
        // 오른쪽 방향키
        if (Input.GetKeyDown(KeyCode.RightArrow) && CardManager.Inst.myCardsCount > 0 && !isCardMoving)
        {
            if (!isCardMoving)
            {
                NumpadCheck();
                StartCoroutine(MoveRight());
            }
            else
            {
                StopAllCoroutines();
                NumpadCheck();
                StartCoroutine(MoveRight());
            }

        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && CardManager.Inst.myCardsCount > 0 && isCardMoving)
        {
            StopAllCoroutines();
            isCardMoving = false;
        }

        //왼쪽 방향키
        if (Input.GetKeyDown(KeyCode.LeftArrow) && CardManager.Inst.myCardsCount > 0 && !isCardMoving)
        {
            if (!isCardMoving)
            {
                NumpadCheck();
                StartCoroutine(MoveLeft());
            }
            else
            {
                StopAllCoroutines();
                NumpadCheck();
                StartCoroutine(MoveLeft());
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) && CardManager.Inst.myCardsCount > 0 && isCardMoving)
        {
            StopAllCoroutines();
            isCardMoving = false;
        }
        #endregion

        // esc, ` 키로 카드 선택 취소
        if ((Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Tab)) && !isCardMoving))
            EnlargeCancle();

        // 카드 내기, 카드 선택 후 스페이스 바로 실행
        if (Input.GetKeyDown(KeyCode.Space) && CardManager.Inst.alreadyEnlarge && !isCardMoving)
        {
            isCardMoving = true;
            isFirstSelect = false;
            CardManager.Inst.TryPutCard();
            // 카드 정보를 넘긴 뒤 행동 실행
        }

        // 카드 드로우
        if (Input.GetKeyDown(KeyCode.Z))
            TurnManager.OnAddCard?.Invoke(true);
    }
    public void EnlargeCancle()
    {
        // 선택 취소
        if (!isCardMoving && CardManager.Inst.alreadyEnlarge)
        {
            for (int i = 0; i < CardManager.Inst.myCardsCount; i++)
                CardManager.Inst.EnlargeCard(false, i);
        }
    }
    IEnumerator ChoiceNum(int num)
    {
        // 키패드 선택
        if (!isCardMoving)
        {
            if (!isFirstSelect)
            {
                isFirstSelect = true;
            }

            isCardMoving = true;
            if (CardManager.Inst.alreadyEnlarge && CardManager.Inst.myCardsCount != 1 && prevNum != num)
                CardManager.Inst.EnlargeCard(false, prevNum);
            CardManager.Inst.EnlargeCard(true, num);
            prevNum = num;
            yield return new WaitForSeconds(0.15f);
            isCardMoving = false;
            isNumpad = true;

        }
    }
    IEnumerator MoveLeft()
    {
        // 왼쪽 이동
        isCardMoving = true;
        //DOTween.KillAll();
        if (CardManager.Inst.alreadyEnlarge && CardManager.Inst.myCardsCount != 1)
            CardManager.Inst.EnlargeCard(false, cardNum);
        if (!isFirstSelect)
        {
            cardNum = 0;
            isFirstSelect = true;
        }
        else
        {
            if (cardNum > 0)
                cardNum--;
            else if (cardNum <= 0)
                cardNum = CardManager.Inst.myCardsCount - 1;
        }
        CardManager.Inst.EnlargeCard(true, cardNum);
        prevNum = cardNum;
        yield return new WaitForSeconds(0.15f);
        isCardMoving = false;
        isNumpad = false;
        StartCoroutine(MoveLeft());
    }
    IEnumerator MoveRight()
    {
        // 오른쪽 이동
        isCardMoving = true;
        //DOTween.KillAll();
        if (CardManager.Inst.alreadyEnlarge && CardManager.Inst.myCardsCount != 1)
            CardManager.Inst.EnlargeCard(false, cardNum);
        if (cardNum < CardManager.Inst.myCardsCount - 1)
            cardNum++;
        else if (cardNum <= CardManager.Inst.myCardsCount - 1)
            cardNum = 0;
        if (!isFirstSelect)
        {
            cardNum = 0;
            isFirstSelect = true;
        }
        //Debug.Log("선택한 카드 번호:" + cardNum);
        CardManager.Inst.EnlargeCard(true, cardNum);
        prevNum = cardNum;
        yield return new WaitForSeconds(0.15f);
        isCardMoving = false;
        isNumpad = false;
        StartCoroutine(MoveRight());
    }
    public void NumpadCheck()
    {
        // 숫자패드, 방향키 중복 입력 방지 체크
        if (!isNumpad)
        {
            prevNum = cardNum;
        }
        else
        {
            cardNum = prevNum;
        }
    }
    public void NumpadExecution(int num)
    {
        // 반복되는 코드 줄이기 위해 함수화
        NumpadCheck();
        StartCoroutine(ChoiceNum(num));
    }
    public void StartGame()
    {
        isCardMoving = true;
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }
    public void Notification(string message, bool myTurn)
    {
        if (!myTurn)
            notificationPanel.Show(message, red);
        else
            notificationPanel.Show(message, yellow);
    }
}
