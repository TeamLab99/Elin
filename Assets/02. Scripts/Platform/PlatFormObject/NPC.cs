using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
public class NPC : InteractObject
{
    [SerializeField] string npcName;

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
    }
    
    [YarnCommand("ttt")]
    public static void CoutLog(string num)
    {
        Debug.Log(num);
        Debug.Log(int.Parse(num));
        Debug.Log("커맨드 함수 작동중");
    }

    [YarnCommand("kkk")]
    public static void Use()
    {
        int cnt = 2;
        Dictionary<int,int> itemList = new Dictionary<int, int>();
        itemList.Add(101, 5);
        itemList.Add(102, 3);
        if (itemList.ContainsKey(101))
        {
            itemList[101] -= cnt;
            Debug.Log(itemList[101]);
        }
        else
        {
            Debug.Log("못찾음");
        }
    }
}
