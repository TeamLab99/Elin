using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public PlayerStat playerStat;
    public PlayerMove playerMove;
    public PlayerAbilityController playerAbilityController;
    private void Awake()
    {
        playerStat = GetComponent<PlayerStat>();
        playerMove = GetComponent<PlayerMove>();
        playerAbilityController = GetComponent<PlayerAbilityController>();
    }
}
