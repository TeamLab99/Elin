using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEventManager : Singleton<PlatformEventManager>
{
    [SerializeField] GameObject appearPlatform;
    [SerializeField] GameObject movePlatform;
    [SerializeField] GameObject enhanceUI;
    [SerializeField] Vector2 ericaSpawnPos;
    GameObject player;
    GameObject brokeBranch;
    PlayerController playerController;
    int idx = -1;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        brokeBranch = GameObject.FindGameObjectWithTag("Branch");
        playerController = player.GetComponent<PlayerController>();
    }

    public void SetEvent()
    {
        idx += 1;
        switch (idx)
        {
            case 0:
                AppearPlatformEvent();
                break;
            case 1:
                DialogueManager.instance.StartDialogue("plusText");
                break;
            case 2:
                movePlatform.GetComponent<MovePlatform>().movePlatform = true;
                break;
            case 3:
                SeeAppleEvent();
                break;
            case 4:
                FallEvent();
                break;
            case 5:
                enhanceUI.SetActive(true);
                DialogueManager.instance.EarnAum();
                break;
        }
    }

    public void AppearPlatformEvent()
    {
        appearPlatform.SetActive(true);
    }

    public void SeeAppleEvent()
    {
        CamerEffect.instance.ChangeGotoCameraMode();
    }

    public void FallEvent()
    {
        brokeBranch.SetActive(false);
        CamerEffect.instance.ChangeLateFollowCamerMode();
    }

    public void EricaSpawn()
    {
        GameObject erica = GameObject.Find("Erica");
        erica.transform.position = ericaSpawnPos;
    }
}
