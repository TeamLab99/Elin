using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuestDataLoader : MonoBehaviour
{
    public TextAsset data;
    private QuestData questData;

    void Start()
    {
        questData = JsonUtility.FromJson<QuestData>(data.text);    
    }

    public Quest GetQuestByID(int quest_id)
    {
        foreach (Quest quest in questData.quests)
        {
            if (quest.questid == quest_id)
            {
                return quest;
            }
        }
        return null;
    }

    public Quest[] GetQuestsByNPCID(int npc_id)
    {
        List<Quest> npcQuests = new List<Quest>();
        foreach (Quest quest in questData.quests)
        {
            if (quest.npcid == npc_id)
            {
                npcQuests.Add(quest);
            }
        }
        return npcQuests.ToArray();
    }
}

[System.Serializable]
public class QuestData
{
    public Quest[] quests;
}

[System.Serializable]
public class Quest
{
    public int npcid;
    public int questid;
    public string[] dialogue;
}