using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


//ġƮ, UI, ��ŷ, ���ӿ���
public class BPGameManager : MonoBehaviour
{
    [SerializeField] NotificationPanel notificationPanel;

    Color32 red = new Color32(255, 0, 0, 255);
    Color32 yellow = new Color32(255, 244, 0, 255);

    int cardNum = 0;

    public bool isCardMoving = false;
    public bool isDraw = false;

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
        if (Input.GetKeyDown(KeyCode.Z))
            TurnManager.OnAddCard?.Invoke(true);

        if (Input.GetKeyDown(KeyCode.X))
            TurnManager.OnAddCard?.Invoke(false);

        if (Input.GetKeyDown(KeyCode.C))
            TurnManager.Inst.EndTurn();

        if (Input.GetKeyDown(KeyCode.V))
            CardManager.Inst.TryPutCard(false);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!isCardMoving)
                StartCoroutine(MoveRight());
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopAllCoroutines();
            isCardMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(!isCardMoving)
                StartCoroutine(MoveLeft());
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopAllCoroutines();
            isCardMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isCardMoving)
            {
                for (int i = 0; i < CardManager.Inst.myCardsCount; i++)
                    CardManager.Inst.EnlargeCard(false, i);
            }
        }
    }

    IEnumerator MoveLeft()
    {
        isCardMoving = true;
        //DOTween.KillAll();

        if (CardManager.Inst.alreadyEnlarge && CardManager.Inst.myCardsCount != 1)
            CardManager.Inst.EnlargeCard(false, cardNum);

        if (cardNum > 0)
            cardNum--;
        else if (cardNum <= 0)
            cardNum = CardManager.Inst.myCardsCount-1;

        CardManager.Inst.EnlargeCard(true, cardNum);

        yield return new WaitForSeconds(0.2f);

        isCardMoving = false;
        StartCoroutine(MoveLeft());
    }

    IEnumerator MoveRight()
    {
        isCardMoving = true;
        //DOTween.KillAll();


        if (CardManager.Inst.alreadyEnlarge&& CardManager.Inst.myCardsCount!=1)
            CardManager.Inst.EnlargeCard(false, cardNum);

        if (cardNum < CardManager.Inst.myCardsCount-1)
            cardNum++;
        else if (cardNum <= CardManager.Inst.myCardsCount-1)
            cardNum = 0;

        Debug.Log("������ ī�� ��ȣ:" + cardNum);

        CardManager.Inst.EnlargeCard(true, cardNum);

        yield return new WaitForSeconds(0.2f);

        isCardMoving = false;
        StartCoroutine(MoveRight());
    }

    public void StartGame()
    {
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
