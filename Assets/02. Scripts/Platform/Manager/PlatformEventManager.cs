using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEventManager : Singleton<PlatformEventManager>
{
    GameObject player;
    GameObject brokeBranch;
    PlayerController playerController;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        brokeBranch = GameObject.FindGameObjectWithTag("Branch");
        playerController = player.GetComponent<PlayerController>();
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

    public void ControlPlayerMove(bool _control)
    {
        playerController.ControlPlayer(_control);
    }
}
