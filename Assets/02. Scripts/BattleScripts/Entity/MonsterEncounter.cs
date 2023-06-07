using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEncounter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        BattleGameManager.instance.StartBattle(gameObject.transform.position);
    }
}
