using UnityEngine;
using System.IO;

public class QuestTest : MonoBehaviour
{
    public TextAsset dialogues;

    private DialogueData data;

    void Start()
    {
        string jsonString = dialogues.text;
        data = JsonUtility.FromJson<DialogueData>(jsonString);
    }

    public Dialogue[] GetDialogueForNPC(string npcID)
    {
        for (int i = 0; i < data.npcs.Length; i++)
        {
            if (data.npcs[i].id == npcID)
            {
                return data.npcs[i].dialogue;
            }
        }
        return null;
    }
}

[System.Serializable]
public class DialogueData
{
    public NPC[] npcs;
}

[System.Serializable]
public class NPC
{
    public string id;
    public Dialogue[] dialogue;
}

[System.Serializable]
public class Dialogue
{
    public string content;
}