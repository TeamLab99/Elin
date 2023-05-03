using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : InteractObject
{
    public int npc_id;
    public int quest_id;
    public int text_idx;
    public Image npcImage;
    public GameObject textBox;
    public Text text;
    private bool isTalking=false;
    private QuestDataLoader questDataLoader;
    private Quest[] questsList;

    void Start()
    {
        questDataLoader = FindObjectOfType<QuestDataLoader>();
        questsList = questDataLoader.GetQuestsByNPCID(npc_id);
    }

    public void StartQuest(int quest_id)
    {
        Quest quest = questDataLoader.GetQuestByID(quest_id);
        if (quest != null && quest.npcid == npc_id)
        {
            // 퀘스트 시작
        }
    }

    public void AllQuestList()
    {
        /*foreach (Quest quest in quests)
        {
        }*/
    }
    public void ShowQuestText()
    {
         if (quest_id >= questsList.Length)
        {
            Debug.Log("실패");
            return;
        }
            
        if (!isTalking)
        {
            textBox.SetActive(true);
            text.text = questsList[quest_id].dialogue[text_idx];
            isTalking = true;
            text_idx++;
        }
        else
        {
            if (text_idx >= questsList[quest_id].dialogue.Length)
            {
                textBox.SetActive(false);
                isTalking = false;
                quest_id++;
                text_idx = 0;
                return;
            }
            text.text = questsList[quest_id].dialogue[text_idx];
            text_idx++;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("대화 가능");
        }
    }
}
