using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//덱 화면을 열기 위한 버튼 스크립트
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
            //열려있을 때 또 누르면 닫히게 했다.
            panel.SetActive(false);
            isOpended = false;
        }
    }
}
