using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public static Battle Inst { get; private set; }

    void Awake() => Inst = this;

    [SerializeField] Entity player;
    [SerializeField] Entity monster;

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
