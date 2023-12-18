using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterMonster : MonoBehaviour
{
    bool isFirstMeet = false;
    [SerializeField] bool isNightMare = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFirstMeet && collision.tag == "Player")
        {
            DialogueManager.instance.NextDialogue("Erica"); // Erica7-8이동

            if (isNightMare)
            {
                BattleManager.instance.StartNightmare(gameObject);
            }
            else
            {
                BattleManager.instance.StartMadSquirrel(gameObject.transform.position);
            }
            
            isFirstMeet = true;
        }
    }
}
