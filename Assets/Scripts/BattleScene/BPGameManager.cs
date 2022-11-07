using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//치트, UI, 랭킹, 게임오버
public class BPGameManager : MonoBehaviour
{
    [SerializeField] NotificationPanel notificationPanel;

    Color32 red = new Color32(255, 0, 0, 255);
    Color32 yellow = new Color32(255, 244, 0, 255);
    
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
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

    public void Notification(string message, bool myTurn)
    {
        if(!myTurn)
            notificationPanel.Show(message, red);
        else
            notificationPanel.Show(message, yellow);
    }
}
