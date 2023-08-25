using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : Singleton<QuestManager>
{
    GameObject questUI;
    Text questUIText;
    
    private void Awake()
    {
        questUI = GameObject.Find("QuestUI");
        questUIText = questUI.GetComponentInChildren<Text>();
    }

    public void Quest<T>(string questType, T detail)
    {
        switch (questType)
        {
            case "Dialogue": // 대화하는 퀘스트
                break;
            case "Kill": // 처치하는 퀘스트
                break;
            case "Gather": // 모아오는 퀘스트
                break;
        }
    }
}
