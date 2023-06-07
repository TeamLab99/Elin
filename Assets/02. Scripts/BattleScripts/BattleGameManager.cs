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
    [SerializeField] Ease ease;
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
        // + 치트 키 입력
    }

    public void StartBattle(Vector3 mobPos)
    {
        player.GetComponent<Player_Move>().enabled = false;
        mainCamera.GetComponent<Camera_Follow>().enabled = false;

        var targetPos = new Vector3(Mathf.Lerp(player.transform.position.x, mobPos.x, 0.5f), mobPos.y, -15);
        mainCamera.transform.DOMove(targetPos, 1.5f).SetEase(ease);

        StartCoroutine(TurnManager2.instance.StartGameCo(uiObject.transform.GetChild(0).gameObject, 2f));
    }

    public void EndBattle()
    {
        player.GetComponent<Player_Move>().enabled = true;
        mainCamera.GetComponent<Camera_Follow>().enabled = true;
        // + ui 종료 애니메이션 재생 후 false
    }
}
