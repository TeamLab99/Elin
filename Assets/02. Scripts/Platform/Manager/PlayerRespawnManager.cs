using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager : Singleton<PlayerRespawnManager>
{
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject warningMessage;
    [SerializeField] Transform respawnPos;

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    public void EmergencyRespawn()
    {
        PlayerStatManager.instance.RespawnPlayer(false, respawnPos);
        warningMessage.SetActive(false);
        gameOverUI.SetActive(false);
        PlayerStatManager.instance.playerDead = false;
    }

    public void FullConditionRespawn()
    {
        if (ItemManager.instance.LoadAum() < 100)
        {
            warningMessage.SetActive(true);
            return;
        }
        PlayerStatManager.instance.RespawnPlayer(true, respawnPos);
        ItemManager.instance.UseAum(100);
        gameOverUI.SetActive(false);
        PlayerStatManager.instance.playerDead = false;
    }

    public void ChangeRespawnPosition(Transform _position)
    {
        respawnPos = _position;
    }
}
