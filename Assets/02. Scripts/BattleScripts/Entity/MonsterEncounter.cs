using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEncounter : MonoBehaviour
{
    bool isFirstMeet = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFirstMeet && collision.tag == "Player")
        {
            DialogueManager.instance.NextDialogue("Erica"); // Erica7-8이동
            BattleGameManager.instance.StartBattle(gameObject.transform.position);
            isFirstMeet = true;
        }
    }
}
