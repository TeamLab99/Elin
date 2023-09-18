using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEventManager : Singleton<PlatformEventManager>
{
    [SerializeField] GameObject appearPlatform;
    GameObject player;
    GameObject brokeBranch;
    PlayerController playerController;
    bool onceAppear = false;
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
                SeeAppleEvent();
                break;
            case 3:
                FallEvent();
                break;
            case 4:
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
}
