using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 치트, UI, 게임 스타트, 오버
public class BattleGameManager : Singleton<BattleGameManager>
{
    [SerializeField] NotificationPanel notificationPanel;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] GameObject monster;
    [SerializeField] Ease ease;

    GameObject battleUI;
    bool isSetting;

    void Start()
    {
        StartCoroutine(GetUI());
    }

    IEnumerator GetUI()
    {
        yield return new WaitForEndOfFrame();
        battleUI = GameObject.FindGameObjectWithTag("UI").transform.Find("Battle").gameObject;
        notificationPanel = battleUI.transform.Find("NotificationPanel").GetComponent<NotificationPanel>();
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
        BattleCardManager.instance.CreatePoolCard();
        BattleMagicManager.instance.SetEntites(player.GetComponent<BattlePlayer>(), monster.GetComponent<BattleMonster>());

        player.GetComponent<Player_Move>().rb.constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<Player_Move>().enabled = false;
        mainCamera.GetComponent<Camera_Follow>().enabled = false;

        var targetPos = new Vector3(Mathf.Lerp(player.transform.position.x, mobPos.x, 0.5f), mobPos.y, -15);
        mainCamera.transform.DOMove(targetPos, 1.5f).SetEase(ease);

        StartCoroutine(BattleTurnManager.instance.StartGameCo(battleUI, monster.GetComponent<BattleMonster>(), 2f));
        
    }

    public void Win()
    {
        player.GetComponent<Player_Move>().enabled = true;
        mainCamera.GetComponent<Camera_Follow>().enabled = true;
        // + ui 종료 애니메이션 재생 후 false
        // + 전투 승리 보상 추가
    }

    public void Lose()
    {
        // 모든 오브젝트 활동 정지
        // 게임 오버 화면 송출
    }

    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }
}
