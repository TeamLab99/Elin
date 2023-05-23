using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
public class NPC : InteractObject
{
    [SerializeField] string npcName;
    public int kkk = 5;
    private void Start()
    {
        dialogueRunner.AddCommandHandler<float>("seeyou",SeeYou);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("대화 가능");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (Input.GetKey(KeyCode.Q)))
        {
            if(!dialogueRunner.IsDialogueRunning)
                dialogueRunner.StartDialogue(npcName);
        }
    }

    public void SeeYou(float k=1)
    {
        Debug.Log(k);
        kkk -= 10;
    }
        
    [YarnCommand("UseItem")]
    public static void CoutLog(int _id)
    {
        Debug.Log(_id);
     }

    [YarnFunction("CheckItem")]
    public static int Use(int cnt)
    {
        int itemAllCnt = 10;
        return itemAllCnt-cnt;
    }
}
