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
    float time=1f;

    private void Start()
    {
        scroll.fillAmount = time;
        //scroll.fillAmount = stamina*0.01f;
        //scroll.fillAmount = 1;
        StartCoroutine(MonsterAttack());
    }

    IEnumerator MonsterAttack()
    {
        if (player.health <= 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        Attack(5, true);

        StartCoroutine(MonsterAttack());
    }

    public void Attack(int num, bool isMine)
    {
        Entity entity = isMine ? player : monster;
        entity.health -= num;
        entity.SetHealth();
    }
    public void Heal(int num, bool isMine)
    {
        Entity entity = isMine ? player : monster;
        entity.health += num;
        entity.SetHealth();
    }
}
