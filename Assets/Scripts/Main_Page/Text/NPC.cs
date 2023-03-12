using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public int npc_id;
    private QuestDataLoader questDataLoader;

    void Start()
    {
        questDataLoader = FindObjectOfType<QuestDataLoader>();
    }

    public void StartQuest(int quest_id)
    {
        Quest quest = questDataLoader.GetQuestByID(quest_id);
        if (quest != null && quest.npcid == npc_id)
        {
            // 퀘스트 시작
        }
    }

    public void ShowQuestList()
    {
        Quest[] quests = questDataLoader.GetQuestsByNPCID(npc_id);
        foreach (Quest quest in quests)
        {
            Debug.Log(quest.dialogue[0]);
            // 퀘스트 목록 표시
        }
    }
}
