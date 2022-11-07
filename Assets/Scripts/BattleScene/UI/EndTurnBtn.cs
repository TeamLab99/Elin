using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTurnBtn : MonoBehaviour
{
    [SerializeField] Sprite active;
    [SerializeField] Sprite inactive;
    [SerializeField] TMP_Text btnText;


    private void Start()
    {
        Setup(false);
        TurnManager.OnTurnStarted += Setup;
    }

    private void OnDestroy()
    {
        TurnManager.OnTurnStarted -= Setup;
    }

    public void Setup(bool isActive)
    {
        GetComponent<Image>().sprite = isActive ? active : inactive;
        GetComponent<Button>().interactable = isActive;
        btnText.color = isActive ? new Color32(255, 195, 90, 255) : new Color32(55, 55, 55, 255);
    }
}
