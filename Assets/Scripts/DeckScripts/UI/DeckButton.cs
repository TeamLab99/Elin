using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckButton : MonoBehaviour
{
    [SerializeField]
    GameObject panel;

    bool isOpended = false;
    public void OpenPanel()
    {
        if (!isOpended)
        {
            panel.SetActive(true);
            isOpended = true;
        }
        else
        {
            panel.SetActive(false);
            isOpended = false;
        }
    }
}
