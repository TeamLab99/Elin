using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

// 치트, UI, 게임 스타트, 오버
public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] NotificationPanel notificationPanel;
    [SerializeField] PlatformUIAnimation platformUI;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] Animator animator;
    [SerializeField] GameObject monster;
    [SerializeField] GameObject nightmarePrefab;
    [SerializeField] Transform monstersTr;
    [SerializeField] Ease ease;
    [SerializeField] Nightmare nightmare;

    GameObject battleUI;
    bool isSetting;
    bool isAnimState;

    public static Action PlatformUIControlForBattle;
    public static Action PlatformUIControlForDialouge;

    public void SetMonster(GameObject monster)
    {
        this.monster = monster;
        animator = monster.GetComponent<Animator>();
    }

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
    
    public void StartMadSquirrel(Vector3 mobPos)
    {
        CardManager.instance.CreatePoolCard();
        MagicManager.instance.SetEntites(player.GetComponent<Player>(), monster.GetComponent<Monster>());
        MobSkillManager.instance.SetEntites(player.GetComponent<Player>(), monster.GetComponent<Monster>());
        
        CamerEffect.instance.StopCamera();
        Managers.Input.PlayerMoveControl(false);

        var targetPos = new Vector3(Mathf.Lerp(player.transform.position.x, mobPos.x, 0.5f), mobPos.y + 5f, -150);
        mainCamera.transform.DOMove(targetPos, 1.5f).SetEase(ease);

        StartCoroutine(TurnManager.instance.StartGameCo(battleUI, monster.GetComponent<Monster>(), player.GetComponent<Player>(), 2f));
    }

    public void StartNightmare(GameObject nightmare)
    {
        monster = nightmare;
        animator = monster.GetComponent<Animator>();
        
        CardManager.instance.ChangeRandomMode(EMagicType.RandomNightmare);
        CamerEffect.instance.StopCamera();
        Managers.Input.PlayerMoveControl(false);

        var targetPos = new Vector3(Mathf.Lerp(player.transform.position.x, nightmare.transform.position.x, 0.5f), nightmare.transform.position.y + 5f, -150);
        mainCamera.transform.DOMove(targetPos, 1.5f).SetEase(ease);

        StartCoroutine(TurnManager.instance.StartGameCoWithNightmare(battleUI, monster.GetComponent<Monster>(), player.GetComponent<Player>(), 2f));
    }

    public void GameOver()
    {
        OnOffUI.instance.endingObject.SetActive(true);
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

    public void ChangeAnim(EMonsterState monsterState)
    {
        switch (monsterState)
        {
            case EMonsterState.Idle:
                animator.SetBool("isHit", false);
                animator.SetBool("isSkill", false);
                animator.SetBool("isAttack", false);
                break;
            case EMonsterState.Attack:
                animator.SetBool("isAttack", true);
                break;
            case EMonsterState.Hit:
                animator.SetBool("isHit", true);
                break;
            case EMonsterState.Skill:
                animator.SetBool("isSkill", true);
                break;
            case EMonsterState.Death:
                animator.SetBool("Death", true);
                break;
        }
    }


    public void GenerateNightmare()
    {
        monster.GetComponent<Nightmare>().SetBattle();
    }

    public Vector3 CalculateLerpPoint()
    {
        return Vector3.Lerp(player.transform.position, monster.transform.position, 0.5f);
    }

    public void BattleEnd()
    {
        battleUI.SetActive(false);
        PlatformUIControlForDialouge?.Invoke();
    }

    public void ResetCamera()
    {
        mainCamera.GetComponent<CamerEffect>().ChangeFollowCameraMode();
    }

    public void StartNightmareBattle()
    {
        monster.GetComponent<Nightmare>().StartBattle();
    }
}
