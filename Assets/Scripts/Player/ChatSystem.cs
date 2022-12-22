using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatSystem : MonoBehaviour
{
    public Queue<string> sentences; //string을 담을 큐 선언
    public string currentSentence;
    public TextMeshPro text;
    public void Ondialogue(string[] lines)
    {
        sentences = new Queue<string>();
        sentences.Clear();
        foreach(var line in lines) {
            sentences.Enqueue(line);
         }
        StartCoroutine(DialogueFlow());
    }
    IEnumerator DialogueFlow()
    {
        yield return null;
        while (sentences.Count > 0)
        {
            currentSentence=sentences.Dequeue();
            text.text = currentSentence;
            yield return new WaitForSeconds(3);
        }
        Destroy(gameObject);
    }

   
}
