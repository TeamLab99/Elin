using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public static Battle Inst { get; private set; }

    void Awake() => Inst = this;

    [SerializeField] Entity player;
    [SerializeField] Entity monster;

    [Header("UI")]
    public Image scroll;
    static float time;
    float maxTime = 3f;


    bool isPlayerDie;

    private void Start()
    {
        //StartCoroutine(MonsterAttack());

        time = maxTime;
        //StartCoroutine(MonsterNormalAttack());
    }

    IEnumerator MonsterNormalAttack()
    {
        if (player.health <= 0)
        {
            StopCoroutine(MonsterNormalAttack());
            yield return null;
        }
        //StartCoroutine(MonsterAttack());

        yield return new WaitForSeconds(3f);
        Attack(2, true);

        StartCoroutine(MonsterNormalAttack());
    }

    public void Attack(int num, bool isMine)
    {
        Entity entity = isMine ? player : monster;

        entity.health -= num;

        if (entity.health <= 0)
        {
            entity.health = 0;
            Debug.Log("게임 오버!");
            isPlayerDie = true;
            BPGameManager.Inst.isCardMoving = true;
            TurnManager.Inst.isLoading = true;
            return;
        }

        entity.SetHealth();
    }
    public void Heal(int num, bool isMine)
    {
        Entity entity = isMine ? player : monster;
        entity.health += num;
        entity.SetHealth();
    }

    private void Update()
    {

        scroll.fillAmount = time / maxTime;

        if (!isPlayerDie)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                time = maxTime;
                EffectManager.Inst.MoveTransform(monster.gameObject, new PRS(monster.transform.position+ Vector3.left *25, Utils.QI, Vector3.one * 1.2f), true, 0.6f);
                Attack(10, true);
            }
        }

    }
}
