using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임의 전체적인 상황을 관리하는 게임 매니저
/// 주로 턴, 키 입력 이벤트, 게임 오버, 치트, UI들을 관리한다.
/// </summary>
public class BPGameManager : MonoBehaviour
{
    public static BPGameManager Inst { get; private set; } // 싱글턴
    void Awake() => Inst = this;

    [SerializeField] Player player;
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] Transform mobSpawnPos;
    GameObject mob;
    Monster monster;

    /*    [SerializeField] NotificationPanel notificationPanel;
    Color32 red = new Color32(255, 0, 0, 255);
    Color32 yellow = new Color32(255, 244, 0, 255);*/

    private void Start()
    {
        StartGame(); // TurnManager에게 시작을 알림.
    }
    private void Update()
    {
        // 키 입력 검사
        InputKey();

#if UNITY_EDITOR
        // 치트키
#endif
    }

    // 키 입력 검사
    private void InputKey()
    {
        #region 키패드 선택
        // 키 입력 판단 && 카드가 움직이고 있지 않을 때 && 카드 개수 검사
        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(CardManager.Inst.MoveToChoiceNum(0));
        if (Input.GetKeyDown(KeyCode.S))
            StartCoroutine(CardManager.Inst.MoveToChoiceNum(1));
        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(CardManager.Inst.MoveToChoiceNum(2));
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(CardManager.Inst.MoveToChoiceNum(3));
        if (Input.GetKeyDown(KeyCode.G))
            StartCoroutine(CardManager.Inst.MoveToChoiceNum(4));
        #endregion

        #region 방향키 선택
        /*        // 오른쪽 방향키
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    StartCoroutine(CardManager.Inst.MoveToArrow(true));
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    StopAllCoroutines();
                    CardManager.Inst.StopCo();
                }

                //왼쪽 방향키
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    StartCoroutine(CardManager.Inst.MoveToArrow(false));
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    StopAllCoroutines();
                    CardManager.Inst.StopCo();
                }*/
        #endregion

        #region 그 외
        // esc, 왼쪽 Shift 키로 카드 선택 취소
        if ((Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.LeftShift))))
            CardManager.Inst.AllEnlargeCancle();

        // 카드 내기, 카드 선택 후 스페이스 바로 실행
        if (Input.GetKeyDown(KeyCode.Space) && !CardManager.Inst.isSelectCardNull())
        {
            StartCoroutine(CardManager.Inst.TryPutCardCorutine());
        }
        #endregion
    }

    // 카드 선택을 막고 TurnManager에게 게임 시작을 알려줌
    public void StartGame()
    {
        SpawnMonster();
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

    public IEnumerator GameOver()
    {
        // 게이지 막기
        monster.SetStopGauge(true);

        // 카드 선택 정지
        TurnManager.Inst.isLoading = true; // 카드 선택 막기
        CardManager.Inst.AllEnlargeCancle(); // 카드 선택 취소


        yield return new WaitForSeconds(2f);
        // 애니메이션 멈추기
        player.gameObject.GetComponent<Animator>().speed = 0f;
        monster.gameObject.GetComponent<Animator>().speed = 0f;

        // 게임 오버 알리기
        TurnManager.Inst.Notification("게임 오버", false);
        Debug.Log("게임 오버!");

        // 씬 이동
        yield return new WaitForSeconds(3f);
        MoveScene();
    }

    public void MoveScene()
    {
        SceneManager.LoadScene(0);
    }

    public void SpawnMonster()
    {
        mob = Instantiate(monsterPrefab, mobSpawnPos.position, Utils.QI, mobSpawnPos);
    }

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
        monster.SetStopGauge(true);
        MobSkillManager.Inst.SetEntites(player, monster);
        MagicManager.Inst.SetEntites(player, monster);

    }

    public void MonsterPause(bool isStop)
    {
        monster.SetStopGauge(isStop);
    }
}
