using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ġƮ, UI, ��ŷ, ���ӿ���
public class BPGameManager : MonoBehaviour
{
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
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }
}
