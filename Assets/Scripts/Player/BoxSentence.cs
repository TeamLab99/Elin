using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSentence : MonoBehaviour
{
    public string[] sentences; //������ ������ �迭
    public Transform boxChatTr;
    public GameObject boxChatPrefab;
    void Start()
    {
        
    }

    public void TalkBox()
    {
        GameObject go = Instantiate(boxChatPrefab);
        go.GetComponent<ChatSystem>().Ondialogue(sentences);
    }

    private void OnMouseDown()
    {
        TalkBox();
    }
    void Update()
    {
        
    }
}
