using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public Text text;
    public string[] firstEvent;
    private int eventSequence = 0;

    private void Update()
    {
        text.text = firstEvent[eventSequence];
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (firstEvent.Length-1 == eventSequence)
            {
                gameObject.SetActive(false);
                eventSequence = 0;
                return;
            }
            else
                eventSequence++;
        }
    }
}
