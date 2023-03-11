using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuestTest : MonoBehaviour
{
    public string jsonFilePath;
    public TextAsset data;
    private AllData datas;
    public Dictionary<string, Dictionary<string, List<string>>> npcQuestDialogueDict;

    public void ShowQuestDialogue(string npcName, string questName)
    {
        List<string> dialogueList = npcQuestDialogueDict[npcName][questName];
        foreach (string dialogue in dialogueList)
        {
            Debug.Log(dialogue);
        }
    }
}

public class AllData
{
    public int progress;
}