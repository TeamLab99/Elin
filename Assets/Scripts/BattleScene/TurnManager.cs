using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("Developer")]
    [SerializeField] [Tooltip("���� �� ��带 ���մϴ�")] ETurnMode eTurnMode;
    [SerializeField] [Tooltip("ī�� ����� �ſ� �������ϴ�")] bool fastMode;
    [SerializeField] [Tooltip("���� ī�� ������ ���մϴ�")] int startCardCount;

    [Header("Properties")]
    public bool myTurn;
    public bool isLoading; // ���� ������ true �ϸ� ī��, ��ƼƼ Ŭ�� ����

    enum ETurnMode { Random, My, Other}
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard;

    void GameSetup()
    {
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);

        switch (eTurnMode)
        {
            case ETurnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;
        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard?.Invoke(false);
            yield return delay05;
            OnAddCard?.Invoke(true);
        }
        StartCoroutine(StartTurnCo());
    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;

        if (myTurn)
            BPGameManager.Inst.Notification("���� ��",myTurn);
        else
            BPGameManager.Inst.Notification("��� ��",myTurn);

        yield return delay07;
        OnAddCard?.Invoke(myTurn);
        yield return delay07;
        isLoading = false;
    }

    public void EndTurn()
    {
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
    }
}
