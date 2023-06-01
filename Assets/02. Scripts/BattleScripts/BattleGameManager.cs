using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 치트, UI, 게임 오버
public class BattleGameManager : Singleton<BattleGameManager>
{
    bool isSetting;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject player;
    GameObject uiObject;

    void Start()
    {
        StartCoroutine(GetUI());
    }

    IEnumerator GetUI()
    {
        yield return new WaitForEndOfFrame();
        uiObject = GameObject.FindGameObjectWithTag("Battle_UI");
    }

    void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isSetting)
        {
            isSetting = true;
            StartBattle();
        }
    }

    public void StartBattle()
    {
        player.GetComponent<Player_Move>().enabled = false;

        var targetPos = player.transform.position - Vector3.right * 10f;
        player.transform.DOMoveX(targetPos.x, 0.5f);
        mainCamera.GetComponent<Camera_Follow>().enabled = false;
        targetPos.z = -15;
        mainCamera.transform.DOMove(targetPos - Vector3.left*5f, 0.5f);

        uiObject.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(TurnManager2.instance.StartGameCo());
    }
}
