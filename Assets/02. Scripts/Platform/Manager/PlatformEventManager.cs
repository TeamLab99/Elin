using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEventManager : Singleton<PlatformEventManager>
{
    GameObject player;
    PlayerController playerController;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    public void ControlPlayerMove(bool _control)
    {
        playerController.ControlPlayer(_control);
    }

    public void CameraEffect()
    {

    }

}
